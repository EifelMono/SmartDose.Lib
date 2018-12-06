using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Observer;
using Rowa.Lib.Log.Configuration;

namespace Rowa.Lib.Log.Appender
{
    /// <summary>
    /// Baseclass for File Logging as Rolling Log
    /// </summary>
    internal abstract class LogAppenderRollingFileBase : LogAppenderBase, IDisposable
    {
        #region Members
        /// <summary>
        /// This is the Template for the LogFilename
        /// </summary>
        protected LogConfigTemplate _fileNameTemplate;

        /// <summary>
        /// Singleton that is used for the FileStreamManager
        /// </summary>
        protected LogFileStreamManager _fileStreamManager; 

        /// <summary>
        /// This is the Current Roll Number of the File. Is Changed when logfile hits Size Limit
        /// </summary>
        protected int _currentCount = 0;
        #endregion

        #region non-public Methods
        /// <summary>
        /// Returns the Size in Bytes of a given File or -1 if it was not successful
        /// </summary>
        /// <param name="fileName">Name and Path of the File to get the Size from</param>
        /// <returns>the Filesize in Bytes or -1 if not successful</returns>
        protected long GetFileSize(string fileName)
        {
            if (!File.Exists(fileName)) return -1;

            try
            {
                return new FileInfo(fileName).Length;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Initializes the Rolling File Counter
        /// </summary>
        protected void InitCount()
        {
            _currentCount = 0;
            var finished = false;

            do
            {
                var fileName = _fileNameTemplate.SetValue("date", DateTime.Today).SetValue("count", _currentCount).ToString();
                var fileSize = GetFileSize(fileName);
                var fileNameCompression = GetCompressionFileName();

                if (fileSize < _config.MaxLogFileSizeInBytes && !File.Exists(fileNameCompression))
                {
                    finished = true;
                }
                else
                {
                    _currentCount++;
                }

            } while (!finished);
        }

        /// <summary>
        /// Returns the FileName including Compression Extension for the given logConfigTemplates
        /// </summary>
        /// <param name="logConfigTemplate"></param>
        /// <returns></returns>
        private string GetCompressionFileName()
        {
            //Get the Value, than Reset the Result 
            string res = _fileNameTemplate.SetValue("compression", Globals.Constants.CompressionExtension).ToString();
            _fileNameTemplate.SetValue("compression", String.Empty);
            return res; 
        }

        /// <summary>
        /// Calculates and Returns the Current Filename of the LogFile
        /// </summary>
        /// <param name="fileName">The Filename of the currently active logfile<</param>
        /// <returns>Wheter a new File is opend with the returned FileName</returns>
        protected string GetFileName(out bool newFileCreated, out string oldFileName)
        {
            string fileName = ""; 
            oldFileName = _fileNameTemplate.ToString();
            fileName = _fileNameTemplate.SetValue("date", DateTime.Today).ToString();
            string fileNameCompression = GetCompressionFileName();

            var fileSize = _fileStreamManager.GetFileSize(fileName);

            if (oldFileName != fileName ||
                fileSize > _config.MaxLogFileSizeInBytes || 
                File.Exists(fileNameCompression))
            {
                InitCount();
                fileName = _fileNameTemplate.SetValue("count", _currentCount).ToString();
                fileSize = _fileStreamManager.GetFileSize(fileName);
            }

            //If the current File Size is 0, the file has not been created yet
            //Header must be written
            if (fileSize == 0)
            {
                newFileCreated =  true;
                //In case old and new FileName are equal, old Filename will be empty
                //-> First Time a File will be created, no old one exists
                if (fileName.Equals(oldFileName))
                {
                    oldFileName = String.Empty; 
                }
            }
            else
            {
                newFileCreated = false;
            }
            return fileName; 
        }
        #endregion


        #region public Methods
        /// <summary>
        /// Initializes the Appender
        /// </summary>
        /// <param name="config">Config used for initialization</param>
        public override void Initialize(AppenderConfiguration config, LogSubject logSubject)
        {
            base.Initialize(config, logSubject);

            _fileNameTemplate = new LogConfigTemplate(_config.FilePath);
            _fileStreamManager = ServiceLocator.Get<LogFileStreamManager>();
            InitCount();
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
                    ServiceLocator.DisposeObject(ref _fileStreamManager);
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
