using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Types
{
    /// <summary>
    /// Log Entry Base class, provided for external usage
    /// Contains all important options that can be set with a LogEntry
    /// ATTENTION: Please be carefull when adding new values and 
    /// add them in the Konstruktor of the LogEntryExt class to 
    /// </summary>
    public class LogEntry
    {
        private Exception _exception;
        private LogLevel _level;
        private string _parentTaskId = "0";
        private string _taskId = "0";
        private string _format;
        private object[] _args;
        private string _module;


        /// <summary>
        /// Exception given to the Entry
        /// </summary>
        public Exception Exception { get => _exception; set => _exception = value; }

        /// <summary>
        /// Loglevel of the Entry
        /// </summary>
        public LogLevel Level { get => _level; set => _level = value; }

        /// <summary>
        /// Oarent Task ID of the Entry
        /// </summary>
        public string ParentTaskId { get => _parentTaskId; set => _parentTaskId = value; }

        /// <summary>
        /// TaskId of the Entry
        /// </summary>
        public string TaskId { get => _taskId; set => _taskId = value; }

        /// <summary>
        /// String Format of the Entry
        /// </summary>
        public string Format { get => _format; set => _format = value; }

        /// <summary>
        /// Additional Arguments of the Entry
        /// </summary>
        public object[] Args { get => _args; set => _args = value; }

        /// <summary>
        /// The Module that was used, Primary the case in PLC Logs 
        /// </summary>
        public string Module { get => _module; set => _module = value; }
    }
}
