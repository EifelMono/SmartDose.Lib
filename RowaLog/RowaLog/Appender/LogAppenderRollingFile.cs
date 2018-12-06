using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Observer;
using Rowa.Lib.Log.Configuration;

namespace Rowa.Lib.Log.Appender
{
    /// <summary>
    /// This Appender does Synchronous File Logging. The Files Are Rolled every Day and Every x Megabytes (Default 99mb).
    /// </summary>
    internal class LogAppenderRollingFile : LogAppenderRollingFileBase
    {
        #region Members
        /// <summary>
        /// Mutex for Thread Safety
        /// </summary>
        private readonly object _mutex = new object();

        /// <summary>
        /// The LogSubject that is used to send Notifications 
        /// </summary>
        private LogSubject _logSubject; 
        #endregion

        #region public Methods
        /// <summary>
        /// Writes a Log Entry into the Logfile
        /// </summary>
        /// <param name="entry">Entry to be written</param>
        public override void Append(LogEntryExt entry)
        {
            if (entry == null) return;

            lock (_mutex)
            {
                var logEntry = GetLogLine(entry);

                string fileName = GetFileName(out bool changed, out string oldFileName);


                int retryCount = 0;
                bool success = false;

                while (!success && retryCount < Globals.Constants.MaxSynchronousRetries)
                {
                    try
                    {
                        var stream = _fileStreamManager.GetStream(fileName);

                        if (stream != null)
                        {
                            if (changed)
                            {
                                //A new FileName is returned, first write the Header 
                                if (!string.IsNullOrEmpty(_config.HeaderString))
                                {
                                    if (!stream.Write(_config.HeaderString)) continue;
                                }
                            }

                            success = stream.Write(logEntry);
                        }
                    } catch(ObjectDisposedException)
                    {
                        //In case the Filestream Manager has been disposed
                        //LogManager has been disposed and someone tries to write to the
                        //Log anyway -> No loggin possible so return
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
                

                //Notify after the Log was Written -> Old Stream should be closed before 
                //Cleanup 
                if(changed)
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
            }
        }

        /// <summary>
        /// Initializes the Appender
        /// </summary>
        /// <param name="config">Config used for Initialization</param>
        public override void Initialize(AppenderConfiguration config, LogSubject logSubject)
        {
            _logSubject = logSubject; 
            base.Initialize(config, logSubject);
        }

        #endregion
    }
}
