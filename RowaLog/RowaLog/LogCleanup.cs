using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;
using Rowa.Lib.Log.Configuration;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Win32.SafeHandles;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Class which implements the logic for background log file compression and log file cleanup.
    /// </summary>
    internal class LogCleanup : IDisposable
    {
        #region ------------- Constants -----------

        /// <summary>
        /// Interval for checking for new log files to cleanup in milliseconds.
        /// </summary>
        private readonly int CleanupIntervalMilliseconds = Globals.Constants.CleanupThreadIntervalMilliseconds;

        /// <summary>
        /// Extension that should be used for the compression Files
        /// </summary>
        private readonly string CompressionExtension = Globals.Constants.CompressionExtension; 
        #endregion

        #region ------------- Members -------------

        /// <summary>
        /// Holds the configuration to use when processing log cleanup.
        /// </summary>
        private List<LogConfiguration> _configurations;

        /// <summary>
        /// List of Files that should be compressed
        /// </summary>
        private Dictionary<string, LogConfiguration> _compressionFiles;

        /// <summary>
        /// Lock that is used for the Files that should be compressed 
        /// </summary>
        private ReaderWriterLockSlim _compressionFilesLock; 

        /// <summary>
        /// Lock that is used to synchronise Operations to the _configurations List
        /// </summary>
        private ReaderWriterLockSlim _configurationLock; 

        /// <summary>
        /// Holds the thread instance which is used to perform log cleanups.
        /// </summary>
        private Thread _logCleanupThread;

        /// <summary>
        /// Event which is used to signal the cleanup thread to come down.
        /// </summary>
        private ManualResetEvent _shutdownEvent;

        /// <summary>
        /// Event which is used to signal wheter a single File should  be 
        /// compressed 
        /// </summary>
        private ManualResetEvent _compressFileEvent;

        /// <summary>
        /// Event that is set everytime the LogCleanup should be processed
        /// </summary>
        private ManualResetEvent _executeLogCleanupEvent;

        /// <summary>
        /// Timer that is used to Execute a Cleanup
        /// </summary>
        private System.Timers.Timer _cleanupIntervallTimer; 

        /// <summary>
        /// Absolute path of the compression binary.
        /// </summary>
        private string _compressorFile;

        /// <summary>
        /// Is true when the logger is initialized
        /// </summary>
        private bool _initialized;

        /// <summary>
        /// Writer that is used to create Diagnostic Messages 
        /// </summary>
        private LogDiagnosticsWriter _diagnosticWriter;

        /// <summary>
        /// Max Amout of Time a Log File is opend 
        /// </summary>
        private int _maxFileLogFileOpenTimeMs; 
        #endregion

        #region ------------- Konstruktors -------------
        /// <summary>
        /// Initializes a new instance of the <see cref="LogCleanup"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configurations to use when processing log cleanup.
        /// </param>
        public LogCleanup(List<LogConfiguration> configurationList)
        {
            //Set Defaults
            InitMembers(); 

            //Check the Input 
            if (configurationList == null || !configurationList.Any())
            {
                throw new ArgumentException("Invalid configurations specified.");
            }

            //The Elements of the List to the internal List -> The Add Method will 
            //check and validate the given config 
            foreach(LogConfiguration config in configurationList)
            {
                AddConfig(config); 
            }

            //Start Thread 
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCleanup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration that should be used in order to perform the Cleanup</param>
        public LogCleanup(LogConfiguration configuration)
        {
            //Set everything to Default 
            InitMembers(); 

            if (configuration == null)
            {
                throw new ArgumentException("Invalid configuration specified.");
            }

            AddConfig(configuration);

            //Start Thread
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the LogCleanup Class 
        /// Configurations that say what to cleanup can be added via the Add 
        /// Method 
        /// Till a configuration is added the Cleanup will effectivly do nothing 
        /// </summary>
        public LogCleanup()
        {
            // Set everything to Default
            InitMembers();
        }
        #endregion

        #region ------------- Events -------------

        /// <summary>
        /// Arguments that will be provided with the FatalError Event in
        /// order to giben a Error message with a product and component 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="product"></param>
        /// <param name="component"></param>
        public delegate void FatalErrorArgs(string message, string product, string component);

        /// <summary>
        /// Event that will be trown when a Fata Error occured in the LogCleanup 
        /// </summary>
        public event FatalErrorArgs LogCleanupFatalError;

        #endregion

        #region ------------- Methods -------------
        #endregion

        #region ------------- Public --------------
        /// <summary>
        /// Initializes the Cleanup Thread.
        /// </summary>
        public void Initialize()
        {
            _configurationLock.EnterWriteLock();

            try
            {
                if (_initialized) return;


                ExtractCompressorAndStartThreadAndTimer();

                _initialized = true;
            }
            finally
            {
                _configurationLock.ExitWriteLock();
            }
            
        }


        /// <summary>
        /// Adds the given Configuration to the internal Configuration list 
        /// If the Configuration has not only Default Values or is null 
        /// The config will not be added
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>Wheter the config was added successfull</returns>
        public bool AddConfig(LogConfiguration configuration)
        {
            bool res = false; 
            //First checks if the config is addable 
            //If so, we get the Lock and try to add the Element to the List 
            if (configuration != null && !ConfigurationIsInvalid(configuration))
            {
                //If no other config with the same Value for Product and Component Exists
                _configurationLock.EnterWriteLock(); 
                if (!_configurations.Any(x => x.Product == configuration.Product 
                                          && x.Component == configuration.Component))
                {
                    try
                    {
                        //Add a deep Copy to the List 
                        _configurations.Add((LogConfiguration)configuration.Clone());
                        res = true;
                    }
                    catch (Exception e)
                    {
                        //Catch return false afterwards
                        _diagnosticWriter.WriteError("Exception in adding Log Configuration" + e.Message);  
                    }
                }
                //Go out of the Log 
                _configurationLock.ExitWriteLock();

            }
            return res; 
        }

        /// <summary>
        /// Zips the given File Asynchron
        /// </summary>
        /// <param name="filepath"></param>
        public void CompressFileAsync(string filepath, LogConfiguration logConfig)
        {
            if (File.Exists(filepath))
            {
                //Add the File to the List of Files that should be compressed
                _compressionFilesLock.EnterWriteLock();
                try
                {
                    _compressionFiles.Add(filepath, logConfig); 
                } finally
                {
                    _compressionFilesLock.ExitWriteLock(); 
                }
                _compressFileEvent.Set(); 
            }
        }

        /// <summary>
        /// Removes the config that matches the given product and Component
        /// </summary>
        /// <returns></returns>
        public bool RemoveConfig(string product, string component)
        {
            try // When _configurationlock is Disposed and still used
            {
                //If the given strings are not empty 
                if (!String.IsNullOrEmpty(product) || !String.IsNullOrEmpty(component))
                {
                    _configurationLock.EnterWriteLock();
                    try
                    {
                        //Remove all that have the configurated Product and Configuration 
                        _configurations.RemoveAll(x => x.Product == product && x.Component == component);
                        return true;
                    }
                    catch
                    {
                        //false will be returned later
                    }
                    finally
                    {
                        _configurationLock.ExitWriteLock();
                    }
                }
            }
            catch
            {
                //this exception only Havens when a Threaded Cleanup is done
            }
            
            return false; 
        }
        #endregion

        #region ------------- Private --------------

        /// <summary>
        /// Sets the Fatal Error event with the given Data 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="product"></param>
        /// <param name="component"></param>
        private void SetFatalError(string message, string product, string component)
        {
            LogCleanupFatalError?.Invoke(message, product, component); 
        }

        /// <summary>
        /// Sets all Default Value of the internal Variables 
        /// </summary>
        private void InitMembers()
        {
            _configurations = new List<LogConfiguration>();
            _compressionFiles = new Dictionary<string, LogConfiguration>(); 

            _configurationLock = new ReaderWriterLockSlim();
            _compressionFilesLock = new ReaderWriterLockSlim(); 

            _shutdownEvent = new ManualResetEvent(false);
            _compressFileEvent = new ManualResetEvent(false);
            //LogCleanup should be executed one before the Intervall hits
            _executeLogCleanupEvent = new ManualResetEvent(true); 

            _compressorFile = string.Empty;

            _diagnosticWriter = new LogDiagnosticsWriter(Globals.Constants.EventViewerCleanupName);

            _maxFileLogFileOpenTimeMs = Globals.Constants.MaxFileOpenTimeMs; 
        }

        /// <summary>
        /// Extracts the Compressor File from the Resources to be used later on 
        /// And Start the Cleanup Thread
        /// </summary>
        private void ExtractCompressorAndStartThreadAndTimer()
        {
            ExtractCompressorFile();

            //Timer that counts the LogCleanupIntervall
            _cleanupIntervallTimer = new System.Timers.Timer
            {
                Interval = CleanupIntervalMilliseconds,
                AutoReset = true
            };
            _cleanupIntervallTimer.Elapsed += TimerElapsed; 
            _cleanupIntervallTimer.Enabled = true;

            // IMPORTANT NOTE:
            // DON'T MAKE THIS THREAD A BACKGROUND THREAD TO "FIX ISSUES" !!!!
            // THIS WILL RESULT IN CATASTROPHIC BEHAVIOR WHEN USING SERVER GC MODE
            // INSTEAD EVERY APPLICATION THAT USES ROWALOG SHOULD CALL Shutdown APPROPRIATLY
            _logCleanupThread = new Thread(new ThreadStart(StartLogCleanup))
            {
                Name = "RowaLog Cleanup Thread"
            };
            _diagnosticWriter.WriteEntry("LogCleanup Thread started"); 
            _logCleanupThread.Start();
        }
        
        /// <summary>
        /// Is called when the Timer counting the Cleanup Intervall is 
        /// Elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _executeLogCleanupEvent.Set();
        }

        /// <summary>
        /// Returns wheter the set configuration object is invalid with his given Values 
        /// </summary>
        /// <param name="config">config that should be checked </param>
        /// <returns>true when the config is Null or has only default values, else false</returns>
        private bool ConfigurationIsInvalid(LogConfiguration config)
        {
            //If all the Values in the Config are Defaults or the LogDirectory is Empty
            if ((config == null) ||
                (config.KeepLogFilesInDays == 0) &&
                (config.KeepAuditFilesInDays == 0) &&
                (config.KeepWwiFilesInDays == 0) &&
                (!config.CompressOldLogFiles) &&
                (!config.CompressOldWwiFiles) ||
                String.IsNullOrEmpty(config.LogDirectory))
            {
                return true; 
            }
            return false; 
        }

        /// <summary>
        /// Extracts the log compressor binary file from the resources.
        /// </summary>
        private void ExtractCompressorFile()
        {
            try
            {
                byte[] compressorBytes = null;
                var thisAssembly = Assembly.GetExecutingAssembly();

                using (var compressorStream = thisAssembly.GetManifestResourceStream("RowaLog.7za.exe"))
                {
                    compressorBytes = new byte[compressorStream.Length];
                    compressorStream.Read(compressorBytes, 0, compressorBytes.Length);
                }

                this._compressorFile = Path.Combine(Environment.GetEnvironmentVariable("TEMP"),
                                                   string.Format("7za-{0}.exe", Guid.NewGuid()));

                using (var compressorFile = File.Create(this._compressorFile))
                {
                    compressorFile.Write(compressorBytes, 0, compressorBytes.Length);
                }
            }
            catch (Exception ex)
            {
                _diagnosticWriter.WriteError(string.Format("Something went wrong when extract compressor. Message: {0}" +
                                                           ", Stacktract: {1}", ex.Message, ex.StackTrace));  
                this._compressorFile = string.Empty;
            }
        }

        /// <summary>
        /// Start the LogClean Thread that performs the LogCleanup or 
        /// Zips Requested Files 
        /// </summary>
        private void StartLogCleanup()
        {
            try
            {
                do
                {
                    try
                    {
                        if (_compressFileEvent.WaitOne(0))
                        {
                            //OK, Compression of single Files should be executed
                            _compressFileEvent.Reset();
                            Dictionary<string, LogConfiguration> files = GetCompressionFileCopy();

                            foreach (string file in files.Keys)
                            {
                                bool success = false;
                                string errorMsg = "";
                                //Try to compress the given Files, max 2 Times 
                                RetryUntilSuccessOrMaxRetries(() =>
                                {
                                    success = TryCompressFile(file, out errorMsg);
                                    if (!String.IsNullOrEmpty(errorMsg))
                                    {
                                        _diagnosticWriter.WriteError(errorMsg);
                                    }
                                    return success;
                                }, 2, _maxFileLogFileOpenTimeMs);

                                if (!success)
                                {
                                    LogConfiguration config = files[file];
                                    if (config != null)
                                    {
                                        SetFatalError(string.Format("Compressing file: {0} failed. Error: {1}", file, errorMsg),
                                                      config.Product, config.Component);
                                    }
                                }

                            }
                            RemoveCompressionFiles(files);
                        }
                        if (_executeLogCleanupEvent.WaitOne(0))
                        {
                            _executeLogCleanupEvent.Reset();
                            ExecuteLogCleanup();
                        }
                    }
                    catch (Exception exc)
                    {
                        _diagnosticWriter.WriteError(string.Format("Something went wrong during the Cleanup Thread Execution. " +
                                                     "Message: {0}, StackTrace: {1}", exc.Message, exc.StackTrace));
                    }
                } while (WaitHandle.WaitAny(new WaitHandle[] { _shutdownEvent, _compressFileEvent, _executeLogCleanupEvent }) != 0);
                //Wait till Shutdown, FileCompression or LogCleanup is requested
            }
            catch (ObjectDisposedException exc)
            {
                _diagnosticWriter.WriteError(string.Format("Events in Cleanup Thread was disposed: " +
                                                           "Message: {0}, StackTrace: {1}", exc.Message, exc.StackTrace));
            }
        }

        /// <summary>
        /// Retrys the given Function until it succeeds or the Timeout hits
        /// </summary>
        /// <param name="task">Task that should be executed, returns a bool to identicate success</param>
        /// <param name="timeSpan">Timespan that gives the Timeout</param>
        /// <param name="sleepTimeMilliseconds">The time the Method waits between the Retrys</param>
        /// <returns></returns>
        private void RetryUntilSuccessOrMaxRetries(Func<bool> task, int retries, int sleepTimeMilliseconds = 500)
        {
            bool success = false;
            int retryCount = 0; 
            while ((!success) && retryCount < retries )
            {
                Thread.Sleep(sleepTimeMilliseconds);
                success = task();
                retryCount++; 
            }
        }

        /// <summary>
        /// Removes the given Files from the Compression File List if they exist there
        /// </summary>
        /// <param name="compressionFiles"></param>
        private void RemoveCompressionFiles(Dictionary<string, LogConfiguration> compressionFiles)
        {
            _compressionFilesLock.EnterWriteLock(); 
            try
            {
                foreach(string file in compressionFiles.Keys)
                {
                    if (_compressionFiles.Keys.Contains(file))
                    {
                        _compressionFiles.Remove(file); 
                    }
                }
            } finally
            {
                _compressionFilesLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns a List that is a Copy of the Compression Files
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, LogConfiguration> GetCompressionFileCopy()
        {
            Dictionary<string, LogConfiguration> copy; 
            _compressionFilesLock.EnterReadLock(); 
            try
            {
                copy = new Dictionary<string, LogConfiguration>(_compressionFiles); 
            } finally
            {
                _compressionFilesLock.ExitReadLock(); 
            }
            return copy; 
        }

        /// <summary>
        /// Executes the log cleanup operations.
        /// </summary>
        private void ExecuteLogCleanup()
        {
            //Copy the List of Configurations
            List<LogConfiguration> configurationsToWork = new List<LogConfiguration>(); 
            _configurationLock.EnterWriteLock();
            try
            {
                configurationsToWork.AddRange(_configurations);
            } catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            } finally
            {
                _configurationLock.ExitWriteLock();
            }

            foreach(LogConfiguration config in configurationsToWork)
            {
                try
                {
                    if (config.KeepLogFilesInDays > 0)
                    {
                        ExecuteFileCleanup(config, "*.log", config.KeepLogFilesInDays);
                        ExecuteFileCleanup(config, "*.log" + CompressionExtension, config.KeepLogFilesInDays);
                    }

                    if (config.KeepAuditFilesInDays > 0)
                    {
                        ExecuteFileCleanup(config, "*.log", config.KeepAuditFilesInDays);
                        ExecuteFileCleanup(config, "*.log" + CompressionExtension, config.KeepAuditFilesInDays);
                    }

                    if (config.KeepWwiFilesInDays > 0)
                    {
                        ExecuteFileCleanup(config, "*.wwi", config.KeepWwiFilesInDays);
                        ExecuteFileCleanup(config, "*.wwi" + CompressionExtension, config.KeepWwiFilesInDays);
                    }

                    if (config.CompressOldLogFiles)
                    {
                        ExecuteFileCompression(config, "*.log");
                    }

                    if (config.CompressOldWwiFiles)
                    {
                        ExecuteFileCompression(config, "*.wwi");
                    }
                }
                catch (Exception ex)
                {
                    _diagnosticWriter.WriteError(string.Format("Error during configuration for product: {0} and component: " +
                                                               "{1}. Message: {2}, StackTrace: {3}", config.Product, config.Component, 
                                                               ex.Message, ex.StackTrace));
                    SetFatalError(string.Format("Unexpected Exception Message: {0}, Stack Trace: {1}", ex.Message, ex.StackTrace)
                                  , config.Product, config.Component); 
                }
            }               
        }
        
        /// <summary>
        /// Recursivly cleans up all outdated files that fit to the specified search pattern.
        /// </summary>
        /// <param name="searchPattern">
        /// The file search pattern to use when select the files to clean up.
        /// </param>
        /// <param name="numDaysKeepFiles">
        /// The number of days to keep files before cleaning them up.
        /// </param>
        private void ExecuteFileCleanup(LogConfiguration config, string searchPattern, uint numDaysKeepFiles)
        {
            var files = GetFiles(config.LogDirectory, searchPattern);
            
            if ((files == null) || (files.Count == 0) || this._shutdownEvent.WaitOne(0))
            {
                return;
            }

            List<string> ignoredFiles = GetIgnoredFilesForCleanup(config);
            files.RemoveAll(x => ignoredFiles.Contains(x));


            foreach (var file in files)
            {
                if (this._shutdownEvent.WaitOne(0))
                {
                    return;
                }                    

                var fileInfo = new FileInfo(file);

                if (fileInfo.LastWriteTime.AddDays(numDaysKeepFiles) >= DateTime.Now)
                {
                    continue;
                }

                TryDeleteFile(file, true);
            }

            // cleanup empty directories
            if (!Directory.Exists(config.LogDirectory)) return;

            string[] dirs = Directory.GetDirectories(config.LogDirectory, "*.*", SearchOption.AllDirectories);

            if ((dirs == null) || dirs.Length == 0) return;

            List<string> directories = new List<string>(dirs);

            foreach (var directory in directories)
            {
                if (this._shutdownEvent.WaitOne(0))
                {
                    return;
                }

                if (IsOldDateDirectory(directory) || GetIsDirectoryEmpty(directory))
                {
                    TryDeleteDirectory(directory);
                }
            }
            
        }

        private bool IsOldDateDirectory(string directory)
        {
            string[] format = { "yyyyMMdd" };
            try
            {
                DirectoryInfo info = new DirectoryInfo(directory);

                if (info.Exists && DateTime.TryParseExact(info.Name, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    return (date.AddDays(7) < DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                _diagnosticWriter.WriteError(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Executes the file compression for all files that fit to the specified pattern.
        /// </summary>
        /// <param name="searchPattern">
        /// The file search pattern to use when select the files to compress.
        /// </param>
        private void ExecuteFileCompression(LogConfiguration config, string searchPattern)
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var files = GetFiles(config.LogDirectory, searchPattern);

            if ((files == null) || (files.Count == 0) || (this._shutdownEvent.WaitOne(0)))
            {
                return;
            }

            List<string> ignoredFiles = GetIgnoredFilesForCleanup(config);
            files.RemoveAll(x => ignoredFiles.Contains(x));


            foreach (var file in files)
            {
                if (this._shutdownEvent.WaitOne(0))
                {
                    return;
                }   

                var fileDate = new FileInfo(file).LastWriteTime;

                if (fileDate < now)
                {
                    TryCompressFile(file, out string message);
                    if (!String.IsNullOrEmpty(message))
                    {
                        _diagnosticWriter.WriteError(message);
                    }
                }

            }
        }

        
        /// <summary>
        /// Gets a Lost of all Files that will be ignored when cleaning up
        /// </summary>
        /// <param name="directory">directory where ignored filed are searched</param>
        /// <param name="searchPattern">Pattern to search for</param>
        /// <returns>a List of all Files that are ignored during Cleanup</returns>
        private List<string> GetIgnoredFilesForCleanup(LogConfiguration config)
        {
            List<string> result = new List<string>();

            foreach (string item in config.CleanupIgnoreFilter)
            {
                List<string> buffer = GetFiles(config.LogDirectory, item);

                result.AddRange(buffer);
            }

            return result.Distinct().ToList();
        }


        /// <summary>
        /// Recursivly gets all files from the specified directory and its sub directories.
        /// </summary>
        /// <param name="directory">
        /// The directory to get the files from.
        /// </param>
        /// <param name="searchPattern">
        /// The file search pattern to use when filter the file list.
        /// </param>
        /// <returns>
        /// List of files in the specified directory.
        /// </returns>
        private static List<string> GetFiles(string directory, string searchPattern)
        {
            if ((string.IsNullOrWhiteSpace(directory)) ||
                (string.IsNullOrWhiteSpace(searchPattern)) ||
                (!Directory.Exists(directory)))
            {
                return new List<string>();
            }

            List<string> files = new List<string>();

            string[] directoryFiles = Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories);

            if ((directoryFiles != null) && (directoryFiles.Length > 0))
            {
                files.AddRange(directoryFiles);
                searchPattern = searchPattern.Replace("*.", "");
                files.RemoveAll(x => !x.EndsWith(searchPattern, StringComparison.InvariantCultureIgnoreCase));
            }

            return files;
        }


        /// <summary>
        /// Tries to delete the specified file.
        /// </summary>
        /// <param name="file">
        /// The file to delete.
        /// </param>
        private bool TryDeleteFile(string file, bool printMessage)
        {
            try
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                if (printMessage)
                {
                    _diagnosticWriter.WriteError(string.Format("Error while deleting file. Message: {0}, StackTrace {1}",
                                                           ex.Message, ex.StackTrace));
                    Trace.WriteLine(ex.Message);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Tries to delete the specified directory.
        /// </summary>
        /// <param name="directory">
        /// The directory to delete.
        /// </param>
        private void TryDeleteDirectory(string directory)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(directory);
                if (info.Exists && info.CreationTime < DateTime.Now.AddMinutes(-5))
                {
                    info.Delete(true);
                }
            }
            catch (Exception ex)
            {
                _diagnosticWriter.WriteError(string.Format("Error deleting Directory. Message: {0}, StackTrace: {1}", 
                                                            ex.Message, ex.StackTrace)); 
            }
        }

        /// <summary>
        /// Tries to compress the specified file.
        /// </summary>
        /// <param name="file">
        /// The file to compress.
        /// </param>
        private bool TryCompressFile(string file, out string errorMessage)
        {
            try
            {
                //Check if Compressor could not be extracted before or if its deleted
                // -> May happen when the Temp Directory is cleared during Execution 
                if (string.IsNullOrEmpty(this._compressorFile) || !File.Exists(_compressorFile))
                {
                    //Try to Extrace Compressor file againg
                    ExtractCompressorFile();
                    if (string.IsNullOrEmpty(this._compressorFile))
                    {
                        errorMessage = "Compressor File could not be extracted";
                        return false;
                    }
                }

                // try to check whether exclusive file access is available
                using (var fileStream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                }

                //Get Creation Time and Change Time of the File 
                DateTime creationTime = File.GetCreationTime(file);
                DateTime changeTime = File.GetLastWriteTime(file); 

                using (var compressor = new Process())
                {
                    compressor.StartInfo.FileName = this._compressorFile;
                    compressor.StartInfo.Arguments = string.Format("a \"{0}" + CompressionExtension + "\" \"{0}\"", file);
                    compressor.StartInfo.CreateNoWindow = true;
                    compressor.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    compressor.StartInfo.RedirectStandardOutput = false;
                    compressor.StartInfo.RedirectStandardInput = true; 
                    compressor.StartInfo.UseShellExecute = false;

                    if (!compressor.Start())
                    {
                        errorMessage = "Compressing failed. Compressor does not start"; 
                        return false; 
                    }

                    try
                    {
                        if (!compressor.HasExited)
                        {
                            compressor.PriorityClass = ProcessPriorityClass.Idle;
                        }
                    } catch (InvalidOperationException)
                    {
                        errorMessage = "Cannot set Process Priority because the Process has exited";
                        return false; 
                    }

                    //In Case the Shutdown Event was set and the Process did not 
                    //terminate properly 
                    if (!WaitTillProcessExitOrResetEventSet(compressor, _shutdownEvent))
                    {
                        errorMessage = "Dispose was called while compressing. Compression was stopped";
                        RetryUntilSuccessOrMaxRetries(() =>
                        {
                            //Do not print message, deleting can be wrong, thats the reason
                            //we try it 2 times
                            return TryDeleteFile(string.Format("{0}" + CompressionExtension, file), false);
                        }, 2, 100); 
                        return false;
                    }

                    if (compressor.ExitCode != 0)
                    {
                       errorMessage = "Proccessor exited with code " + compressor.ExitCode;
                       TryDeleteFile(string.Format("{0}" + CompressionExtension, file), true);
                       return false; 
                    }

                    //Change Creation and Change Time of the new compressed file
                    File.SetCreationTime(file + CompressionExtension, creationTime);
                    File.SetLastWriteTime(file + CompressionExtension, changeTime); 

                    bool res =  TryDeleteFile(file, true);

                    //Message wheter the File Compression succeed or not 
                    _diagnosticWriter.WriteEntry(string.Format("TryCompress File finished. File: {0} Success: {1}", file, res));

                    errorMessage = "";
                    if (!res)
                    {
                        errorMessage = "Deleting not compressed File failed";
                    } 
                    return res; 
                }
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("message: {0}, StackTrace: {1}", ex.Message, ex.StackTrace); 
                return false; 
            }
        }

        /// <summary>
        /// Waits till either the Process is Exited or the Manual ResetEvent was set
        /// returns Wheter the Process existed
        /// </summary>
        /// <param name="process"></param>
        /// <param name="resetEvent"></param>
        private bool WaitTillProcessExitOrResetEventSet(Process process, ManualResetEvent resetEvent)
        {
            int res;
            using (SafeWaitHandle handle = new SafeWaitHandle(process.Handle, false))
            using (ManualResetEvent processReset = new ManualResetEvent(false))
            {
                processReset.SafeWaitHandle = handle;
                res = WaitHandle.WaitAny(new WaitHandle[] { processReset, resetEvent });
            }
            
            
            //OK -> Index 0 means Process was first, else ResetEvent was set 
            if (res == 0)
            {
                return true; 
            } else
            {
                process.StandardInput.Close();
                //Process has not existed after 100 Milliseconds
                if (!process.WaitForExit(100))
                {
                    //Kill the Process
                    process.Kill(); 
                }
                return false; 
            }
        }

        /// <summary>
        /// Determines if a Directory is empty without loading all of it's contents
        /// </summary>
        /// <param name="path">directory to be checked</param>
        /// <returns>true if a Directory is empty</returns>
        private bool GetIsDirectoryEmpty(string path)
        {
            if (!Directory.Exists(path)) return false;

            IEnumerable<string> dirFiles = Directory.EnumerateFileSystemEntries(path);
            using (IEnumerator<string> en = dirFiles.GetEnumerator())
            {
                return !en.MoveNext();
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_cleanupIntervallTimer != null)
                    {
                        _cleanupIntervallTimer.Enabled = false;
                    }
                    
                    if (this._logCleanupThread == null)
                    {
                        this._shutdownEvent.Dispose();
                        this._compressFileEvent.Dispose();
                        this._executeLogCleanupEvent.Dispose();
                        _configurationLock.Dispose();
                        _compressionFilesLock.Dispose();
                        _diagnosticWriter.WriteEntry("Log Cleanup was not running, disposing finished.");
                        return;
                    }

                    _diagnosticWriter.WriteEntry("Dispose Cleanup Thread requested");
                    this._shutdownEvent.Set();
                    this._logCleanupThread.Join(200);
                    _diagnosticWriter.WriteEntry("Cleanup Thread Disposed");
                    this._shutdownEvent.Dispose();
                    this._compressFileEvent.Dispose();
                    this._executeLogCleanupEvent.Dispose();

                    this._logCleanupThread = null;
                    this._configurationLock.Dispose();
                    _compressionFilesLock.Dispose();
                    if (_cleanupIntervallTimer != null)
                    {
                        _cleanupIntervallTimer?.Stop();
                        _cleanupIntervallTimer?.Dispose();
                    }

                    if (!string.IsNullOrEmpty(this._compressorFile))
                    {
                        TryDeleteFile(this._compressorFile, true);
                        _diagnosticWriter.WriteEntry("Tried to delete Compressdor file");
                    }
                }

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
