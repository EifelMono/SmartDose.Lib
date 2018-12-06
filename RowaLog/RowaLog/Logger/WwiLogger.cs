using Rowa.Lib.Log.Configuration;
using Rowa.Lib.Log.Observer;
using Rowa.Lib.Log.Types;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Class which implements a simple file based WWI logger.
    /// </summary>
    internal class WwiLogger : IWwi
    {
        #region Constants

        /// <summary>
        /// Regular expression to replace invalid characters from a specified file name.
        /// </summary>
        private const string InvalidCharRegEx = @"[\/:\*\?""\<\>\|]";

        /// <summary>
        /// Template for the WWI file path.
        /// </summary>
        private const string WwiFilePathTemplate = "{0}\\{1:yyyyMMdd}.{2}.wwi";

        /// <summary>
        /// Linebreak for the end of a WWI logfile entry.
        /// </summary>
        private static byte[] WwiLogEntryEnd = new byte[1] { 10 };

        #endregion

        #region Members

        /// <summary>
        /// Manager for Filestreams
        /// </summary>
        private LogFileStreamManager _fileStreamManager;

        /// <summary>
        /// The directory for the WWI files. 
        /// </summary>
        private readonly string _directory = string.Empty;

        /// <summary>
        /// Appendix of every WWI file name.
        /// </summary>
        private readonly string _fileNameAppendix = string.Empty;

        /// <summary>
        /// The Subject that should be used to report Errors
        /// </summary>
        private readonly LogSubject _subject; 

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="WwiLogger"/> class.
        /// </summary>
        /// <param name="directory">The directory for the WWI files.</param>
        /// <param name="description">The description of the logged communication.</param>
        /// <param name="remoteAddress">The remote address of the communication partner.</param>
        /// <param name="port">The port which is used for the communication.</param>>
        public WwiLogger(LogFileStreamManager fileStreamManager, string directory, string description, string address, ushort port, LogSubject subject)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("Invalid directory specified.");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Invalid description specified.");
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Invalid address specified.");
            }

            _subject = subject; 

            _directory = directory;
            _fileNameAppendix = string.Format("{0}.{1}_{2}", description, address, port);
            _fileNameAppendix = Regex.Replace(this._fileNameAppendix, InvalidCharRegEx, "_");

            _fileStreamManager = fileStreamManager;
        }

        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="isIncommingMessage">If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.</param>
        public void LogMessage(string message, bool isIncommingMessage = true)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            LogMessage(Encoding.UTF8.GetBytes(message), isIncommingMessage);
        }

        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="isIncommingMessage">If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.</param>
        public void LogMessage(byte[] message, bool isIncommingMessage = true)
        {
            if (message == null)
            {
                return;
            }

            LogMessage(message, 0, message.Length, isIncommingMessage);
        }

        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="index">The index in the message at which logging begins.</param>
        /// <param name="length">The number of bytes to log from the message array.</param>
        /// <param name="isIncommingMessage">If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void LogMessage(byte[] message, int index, int length, bool isIncommingMessage = true)
        {
            if ((message == null) || ((index + length) > message.Length))
            {
                return;
            }


            byte[] header = Encoding.UTF8.GetBytes(string.Format("{0:HH:mm:ss,fff} {1}:",
                                                    DateTime.Now,
                                                    isIncommingMessage ? "R" : "S"));

            string filePath = GetWwiFilePath();
            byte[] buffer;

            using (MemoryStream builder = new MemoryStream())
            {
                builder.Write(header, 0, header.Length);
                builder.Write(message, index, length);
                builder.Write(WwiLogger.WwiLogEntryEnd, 0, WwiLogger.WwiLogEntryEnd.Length);
                buffer = builder.ToArray();
            }

            int retryCount = 0;
            bool success = false;

            while (!success && retryCount < Globals.Constants.MaxSynchronousRetries)
            {
                try
                {
                    var file = _fileStreamManager.GetStream(filePath);

                    if (file == null) continue;
                    success = file.Write(buffer, 0, buffer.Length);
                } catch(ObjectDisposedException)
                {
                    //In Case the FileStream Manager has allready been disposed 
                    //This happens in case everything has been disposed with LogManager.Dispose
                    //But someone still trys to write to the Log
                    success = true; 
                    break; 
                }
                finally
                {
                    if (!success)
                    {
                        retryCount++;
                        Thread.Sleep(10);
                    }
                }   
            }
        }
    

        /// <summary>
        /// returns the current target path for the wwi file
        /// </summary>
        /// <returns>target wwi file</returns>
        private string GetWwiFilePath()
        {
            return string.Format(WwiLogger.WwiFilePathTemplate,
                                            _directory,
                                            DateTime.Now,
                                            _fileNameAppendix);
            
        }
        #endregion

        #region IDisposable Support
        /// <summary>
        /// Dummy Dispose for backward compatability
        /// </summary>
        public void Dispose()
        {
            //Dummy Dispose -> Historical reasons and backwards compatibility 
        }
        #endregion

    }
}
