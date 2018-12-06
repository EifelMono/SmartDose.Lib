using Rowa.Lib.Log.Observer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Rowa.Lib.Log.Types
{
    /// <summary>
    /// Class to use a Filestream for logging
    /// </summary>
    internal class LogFileStreamContainer : IDisposable
    {
        /// <summary>
        /// Synchronization Object for Thread Safety
        /// </summary>
        private readonly object _mutex = new object();

        /// <summary>
        /// Represents the last use of the Stream
        /// </summary>
        private DateTime _lastTouch = DateTime.Now;

        /// <summary>
        /// Filestream to write into the Log
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// subject for error reporting
        /// </summary>
        private readonly LogSubject _subject;

        /// <summary>
        /// Represents the last use of the Stream
        /// </summary>
        public DateTime LastTouch => _lastTouch;

        /// <summary>
        /// String Encoding of the Log
        /// </summary>
        private readonly Encoding _logEncoding;

        /// <summary>
        /// The Size of the Log File
        /// </summary>
        public long Size => GetSize();

        /// <summary>
        /// Default Class constructor
        /// </summary>
        /// <param name="subject">subject for error reporting</param>
        /// <param name="filepath">path to the Logfile that needs to be used</param>
        /// <param name="logEncoding">encoding of the Logfile</param>
        public LogFileStreamContainer(LogSubject subject,  string filepath, Encoding logEncoding)
        {
            _subject = subject;
            _logEncoding = logEncoding;

            PrepareDirectory(filepath);

            _stream = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.Read);

            Touch();

            if (!_stream.CanWrite)
            {
                throw new IOException("Can not write to Stream!");
            }
        }

        /// <summary>
        /// Prepares / Creates the Log Directory
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private void PrepareDirectory(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new DirectoryNotFoundException($"LogDirectory Empty ({filePath})");
            }

            var dirName = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(dirName))
            {
                throw new DirectoryNotFoundException($"LogDirectory Invalid ({filePath})");
            }

            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
        }

        /// <summary>
        /// Resets the last used (Touch) Variable for the Class
        /// </summary>
        private void Touch()
        {
            lock (_mutex)
            {
                _lastTouch = DateTime.Now;
            }
        }

        /// <summary>
        /// Returns the current Filesize
        /// </summary>
        /// <returns>current Filesize</returns>
        private long GetSize()
        {
            lock (_mutex)
            {
                try
                {
                    return _stream.Length;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Writes a Log Entry to the Given File
        /// </summary>
        /// <param name="value">Entry to be written to the file</param>
        /// <returns>true if successful</returns>
        public bool Write(string value)
        {
            byte[] buffer = _logEncoding.GetBytes(value);

            return Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes a Log Entry to the Given File
        /// </summary>
        /// <param name="value">Entry to be written to the file</param>
        /// <param name="offset">offset in value to write from</param>
        /// <param name="count">number of bytes to write</param>
        /// <returns></returns>
        public bool Write(byte[] value, int offset, int count)
        {
            lock (_mutex)
            {
                if (_stream == null) return false;

                try
                {
                    _stream.Write(value, offset, count);
                    _stream.Flush();
                    Touch();

                    return true;
                }
                catch(ObjectDisposedException)
                {
                    //Known exception that can happen -> Stream will be opend again and then 
                    //Writing to the file will be retried 
                }
                catch (Exception e)
                {
                    _subject.ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogStreamIOError, Method = "Write", Message = e.Message });
                }

                return false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            lock (_mutex)
            {
                if (!disposedValue)
                {
                
                    if (disposing)
                    {
                        _stream.Dispose();
                    }
                
                    disposedValue = true;
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
