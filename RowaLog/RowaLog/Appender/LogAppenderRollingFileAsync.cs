using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Observer;

namespace Rowa.Lib.Log.Appender
{
    /// <summary>
    /// This Appender does Asynchronous File Logging. The Files Are Rolled every Day and Every x Megabytes (Default 99mb).
    /// </summary>
    internal class LogAppenderRollingFileAsync : LogAppenderRollingFileBase
    {
        #region Members
        /// <summary>
        /// Loop for Logging
        /// </summary>
        private LogAsyncLoop _logLoop;

        /// <summary>
        /// The Subject of the LogObserver used to send Notificatios
        /// </summary>
        private LogSubject _logSubject; 
        #endregion

        #region non-public Methods

        /// <summary>
        /// Log one Log string
        /// </summary>
        /// <param name="entry">Entry to be written</param>
        /// <returns>Wheter the Writing of the Log was successfull</returns>
        private bool DoWriteLog(LogEntryExt entry)
        {
            //This action will be called multiple times, 
            //THe GetFileName depends on the FileSize, if no Header could be written, no file
            //Will be created -> The FileName will return true till the file was successfully 
            //created 
            string fileName = GetFileName(out bool changed, out string oldFileName);
            LogFileStream stream = null; 
            try
            {
                stream = _fileStreamManager.GetStream(fileName);
            } catch (ObjectDisposedException)
            {
                //The LogFileStreamManager has been disposed
                //Therefore the Log could not be written
                return false; 
            }

            if (stream == null) return false;

            if (changed)
            {
                //New Name returned, first Write the Header
                if (!string.IsNullOrEmpty(_config.HeaderString))
                {
                    //If it was not successfull, return to try again, 
                    //If it was successfull, continue writing the LogEntry
                    if (!stream.Write(_config.HeaderString))
                    {
                        return false;
                    }
                }
            }

            var logString = GetLogLine(entry);

            bool returnvalue = stream.Write(logString);

            //Notify after the Writing with old and new FileName 
            if (changed)
            {
                //Set Notification that a new LogFile will be processed
                _logSubject.Notify(new Notification(LogEventType.LogIO,
                                                    new LogIOEventArgs()
                                                    {
                                                        Type = LoggerIOEventType.NewLogFileCreated,
                                                        NewFileName = fileName,
                                                        OldFileName = oldFileName,
                                                    }));
            }
            return returnvalue; 
        }

        /// <summary>
        /// Adds a LogEntry to the LogQueue
        /// </summary>
        /// <param name="entry">Entry to be added</param>
        /// <returns>true if Adding Successful</returns>
        private bool DoEnqueue(LogEntryExt entry)
        {
            var action = new LogAction
            {
                Action = DoWriteLog,
                Entry = entry
            };

            return _logLoop.TryEnqueue(action);
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

            _logSubject = logSubject; 

            _logLoop = ServiceLocator.Get<LogAsyncLoop>(new Guid("19fbd19b-44ab-42b3-b4bd-18e6edbe8635"));
            _logLoop.Initialize();
        }

        /// <summary>
        /// adds A LogEntry to the Queue for future logging
        /// </summary>
        /// <param name="entry">Entry to be written</param>
        public override void Append(LogEntryExt entry)
        {
            if (entry == null) return;

            if (DoEnqueue(entry)) return;

            long amountCleared = _logLoop.Clear();
            DoEnqueue(new LogEntryExt
            (
                new LogEntry
                {
                    Level = LogLevel.Fatal,
                    Format = $"{amountCleared} Log entries Deleted from Log Queue."
                },
                _logSubject
            ));

            ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogQueueFull, Message = "Log Queue is Full!" });
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _logLoop.WaitLoopEmpty(100);
                    ServiceLocator.DisposeObject(ref _logLoop);
                }

                base.Dispose(disposing);

                disposedValue = true;
            }
        }
        #endregion
    }
}
