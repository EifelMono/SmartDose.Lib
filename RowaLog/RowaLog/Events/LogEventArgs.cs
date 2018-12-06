using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Base Class for Log Event
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Name of the Logger that threw this Event
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// Name of the Appender that threw this Event
        /// </summary>
        public string AppenderName { get; set; }

        /// <summary>
        /// Message of the Event
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Class for Logging Error Events
    /// </summary>
    public class LogErrorEventArgs : LogEventArgs
    {
        /// <summary>
        /// Type of the Logging Error
        /// </summary>
        public LoggerError Error { get; set; }

        /// <summary>
        /// The Exact Method the Error occured 
        /// </summary>
        public string Method { get; set; }
    }

    public class LogIOEventArgs : LogEventArgs
    {
        /// <summary>
        /// The Type of LogIOEvent 
        /// </summary>
        public LoggerIOEventType Type { get; set; }

        /// <summary>
        /// An string holding the old Filename in 
        /// case a NewLogFileCreated is called
        /// </summary>
        public string OldFileName { get; set; }

        /// <summary>
        /// An string holding the new Filename in 
        /// case a NewLogFileCreated is called
        /// </summary>
        public string NewFileName { get; set; }
    }
}
