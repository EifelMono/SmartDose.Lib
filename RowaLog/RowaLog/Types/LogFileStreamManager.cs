using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rowa.Lib.Log.Events;
using System.Diagnostics;
using Rowa.Lib.Log.Observer;

namespace Rowa.Lib.Log.Types
{
    /// <summary>
    /// This is a Filestream for Logging. This Stream tries to stay open for a while to make sure no other Log Entry has to be written before it closes.
    /// This is faster than always opening/Closing the File
    /// </summary>
    internal class LogFileStreamManager : IDisposable
    {
        #region Members
        /// <summary>
        /// Stores all Filestreams of the Class
        /// </summary>
        private readonly Dictionary<string, LogFileStreamContainer> _fileStreams = new Dictionary<string, LogFileStreamContainer>();

        /// <summary>
        /// Defines how long the Log file should stay open after last logging
        /// </summary>
        private int _maxFileOpenTimeMs;

        /// <summary>
        /// Mutex to Make Stream Thread safe
        /// </summary>
        private readonly object _mutex = new object();

        /// <summary>
        /// String Encoding of the Log
        /// </summary>
        private Encoding _logEncoding;

        /// <summary>
        /// Token To cancel the Log Writing
        /// </summary>
        private readonly CancellationTokenSource _cancelStreamOperationToken = new CancellationTokenSource();

        /// <summary>
        /// This Threads removes ununsed File Handles
        /// </summary>
        private Thread _cleanupThread;

        /// <summary>
        /// This Value indicates if the FileStream is initialized
        /// </summary>
        private bool _initialized;

        /// <summary>
        /// The Observer subject that is used for Notifications 
        /// </summary>
        private LogSubject _logSubject;
        #endregion

        #region non-public Methods        
        /// <summary>
        /// Returns the Current File Stream. If the currently opened File Stream is invalid. It returns a new one
        /// </summary>
        /// <param name="filePath">Path of the File to be opened</param>
        /// <returns>Filestream to write into</returns>
        private LogFileStreamContainer InitContainer(string logFilePath)
        {
            LogFileStreamContainer container = null;

            lock (_mutex)
            {
                try
                {
                    try
                    {
                        container = new LogFileStreamContainer(_logSubject, logFilePath, _logEncoding);
                        return container;
                    }
                    catch (UnauthorizedAccessException e)
                    {
                       _logSubject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogFileAccessDenied, Method = "Init", Message = e.Message });
                    }
                    catch (ArgumentException e)
                    {
                        _logSubject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogStreamIOError, Method = "Init", Message = e.Message });
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        _logSubject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogDirectoryCreationFailed, Method = "Init", Message = $"LogDirectory not found ({logFilePath}). {e.Message}" });
                    }
                    catch (PathTooLongException e)
                    {
                        _logSubject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogStreamIOError, Method = "Init", Message = $"Path too long ({logFilePath}). {e.Message}" });
                    }
                    catch (IOException e)
                    {
                        _logSubject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogStreamIOError, Method = "Init", Message = e.Message });
                    }
                    catch (SecurityException e)
                    {
                        _logSubject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogFileAccessDenied, Method = "Init", Message = e.Message });
                    }
                    catch (NotSupportedException e)
                    {
                        _logSubject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogFileAccessDenied, Method = "Init", Message = e.Message });
                    }
                }
                catch (Exception e)
                {
                    container?.Dispose();
                    _logSubject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogFileAccessDenied, Method = "Init", Message = e.Message });
                }

            }

            return null;
        }


       

        /// <summary>
        /// Waits for the Log File to be closed after inactivity
        /// </summary>
        private void WaitCloseFileStream()
        {
            _cleanupThread = new Thread(() =>
            {
                do
                {
                    lock (_mutex)
                    {
                        List<string> toDelete = new List<string>();
                        foreach (var container in _fileStreams)
                        {
                            if ((DateTime.Now - container.Value.LastTouch).TotalMilliseconds > _maxFileOpenTimeMs)
                            {
                                container.Value.Dispose();
                                toDelete.Add(container.Key);
                            }
                        }

                        foreach (var value in toDelete)
                        {
                            _fileStreams.Remove(value);
                        }
                    }                  
                    Thread.Sleep(10);

                } while (!_cancelStreamOperationToken.IsCancellationRequested);
            });

            _cleanupThread.Name = "RowaLog FileStreamManager Cleanup Thread";
            _cleanupThread.Start();
        }
        #endregion


        #region public Methods
        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="logEncoding">Ecoding of the Filestream</param>
        /// <param name="maxFileOpenTimeMs">Maximum Open Time of the Log</param>
        public void Initialize(Encoding logEncoding,LogSubject logSubject, int maxFileOpenTimeMs)
        {
            lock (_mutex)
            {
                if (_initialized) return;

                _logSubject = logSubject; 
                _logEncoding = logEncoding;
                _maxFileOpenTimeMs = maxFileOpenTimeMs;

                WaitCloseFileStream();
                _initialized = true;
            }
        }


        /// <summary>
        /// Returns the Current File Stream. If the currently opened File Stream is invalid. It returns a new one
        /// </summary>
        /// <param name="filePath">Path of the File to be opened</param>
        /// <returns>Filestream to write into, Null when onlyFromCache is true and Cache does not contain 
        /// a FileStream for the given FilePath</returns>
        public LogFileStream GetStream(string filePath)
        {
            LogFileStreamContainer container = null;

            lock (_mutex)
            {
                if (disposedValue) throw new ObjectDisposedException(this.ToString()); 

                if (!_fileStreams.ContainsKey(filePath))
                {
                    container = InitContainer(filePath);

                    if (container == null) return null;

                    _fileStreams.Add(filePath, container);
                }
                else
                {
                    container = _fileStreams[filePath];
                }
            }

            return new LogFileStream(container);
        }


        /// <summary>
        /// Returns the length of a given file, if the file exists or zero if file does not exist.
        /// </summary>
        /// <param name="filePath">Path of the File to be opened</param>
        /// <returns></returns>
        public long GetFileSize(string filePath)
        {
            lock (_mutex)
            {
                if (_fileStreams.ContainsKey(filePath))
                {
                    return _fileStreams[filePath].Size;
                }
                else
                {
                    try
                    {
                        FileInfo file = new FileInfo(filePath);
                        return file.Length;
                    }
                    catch (IOException)
                    {
                        return 0;
                    }
                }
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
                    _cancelStreamOperationToken.Cancel();
                    _cleanupThread?.Join(100);
                    _cleanupThread = null;

                    lock (_mutex)
                    {
                        foreach (var container in _fileStreams)
                        {
                            container.Value.Dispose();
                        }

                        _fileStreams.Clear();

                        _cancelStreamOperationToken.Dispose();

                        disposedValue = true;
                    }
                }
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
