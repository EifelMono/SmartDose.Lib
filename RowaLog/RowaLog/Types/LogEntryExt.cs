using Rowa.Lib.Log.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Rowa.Lib.Log.Types
{
    /// <summary>
    /// This class is a Extended Version of the LogEntry class and for internal 
    /// use only. It contains all Values that should not be visible or editable 
    /// from the outside 
    /// </summary>
    internal class LogEntryExt
    {
        #region ------------- Fields -------------
        /// <summary>
        /// Object of the basic LogEntry class 
        /// </summary>
        private readonly LogEntry _base;

        /// <summary>
        /// Subject of the Log 
        /// </summary>
        private readonly LogSubject _logSubject; 
        #endregion

        #region ------------- Konstruktor -------------
        /// <summary>
        /// Empty Konstruktor, creates LogEntryExtended Object 
        /// having Default Values for Member
        /// </summary>
        public LogEntryExt(LogSubject subject)
        {
            _base = new LogEntry();
            _logSubject = subject; 
        }

        /// <summary>
        /// Creates a new object of the LogEntryExtended class, 
        /// containing the given values from the LogEntry class
        /// </summary>
        /// <param name="entry"></param>
        public LogEntryExt(LogEntry entry, LogSubject subject)
        {
            _base = entry;
            _logSubject = subject; 
        }
        #endregion

        #region ------------- Member -------------
        /// <summary>
        /// Returns the current Object of LogEntry that is used
        /// as a BaseClass for this Extensions
        /// </summary>
        public LogEntry Base => _base; 

        /// <summary>
        /// Time at which the Entry was made
        /// </summary>
        public DateTime TimeStamp = DateTime.Now;

        /// <summary>
        /// Thread from Which the Entry was Made
        /// </summary>
        public string ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

        /// <summary>
        /// Actual Log Message of the Log Entry
        /// </summary>
        public string LogMessage
        {
            get
            {
                if (_base.Args != null && _base.Args.Length > 0)
                {
                    try
                    {
                        return string.Format(_base.Format, _base.Args);
                    }
                    catch
                    {
                        _logSubject.Notify(new Notification(LogEventType.Error, new LogErrorEventArgs()
                        {
                            Error = LoggerError.FormatException,                  
                        }));    
                    }
                }

                return _base.Format;
            }
        }
        #endregion
    }
}
