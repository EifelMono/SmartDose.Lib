using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Rowa.Lib.Log.Appender;
using Rowa.Lib.Log.Events;
using Rowa.Lib.Log.Extensions;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Observer;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Delivers Basic Functionality to all Log Appenders
    /// </summary>
    internal abstract class LogAppenderBase : ILogAppender
    {
        #region Members
        /// <summary>
        /// Log Template to be used toconvert the Log Entry to a string
        /// </summary>
        protected LogConfigTemplate _logTemplate;

        /// <summary>
        /// Configuration used for logging
        /// </summary>
        protected AppenderConfiguration _config;

        /// <summary>
        /// Observer subject that is used for Notify in case of e.g. Errors 
        /// </summary>
        protected LogSubject _subject;  
        #endregion

        #region non-public Methods
        /// <summary>
        /// Initializes the Template that is used to create the Entries
        /// </summary>
        /// <param name="inputFormat">base template to use</param>
        protected virtual void InitLogTemplate(LogConfigTemplate inputFormat)
        {
            _logTemplate = new LogConfigTemplate(inputFormat);

            _logTemplate.SetValue("logger", _config.LoggerName);
            _logTemplate.SetValue("newline", "\r\n");
        }

        /// <summary>
        /// This is called when an Error occurs during logging
        /// </summary>
        /// <param name="args">the Error that's to be raised</param>
        protected void ExecuteLoggerError(LogErrorEventArgs args)
        {
            if (args == null) return;

            args.LoggerName = _config.LoggerName;
            args.AppenderName = _config.Name;

            _subject.Notify(new Notification(LogEventType.Error, args)); 
        }

        /// <summary>
        /// Turns a LogEntry into a string
        /// </summary>
        /// <param name="entry">LogEntry to be converted</param>
        /// <returns>resulting log string</returns>
        protected virtual string GetLogLine(LogEntryExt entry)
        {
            _logTemplate.SetValue("date", entry.TimeStamp);
            _logTemplate.SetValue("threadid", entry.ThreadId);
            _logTemplate.SetValue("loglevel", entry.Base.Level.ToString().ToUpper());
            _logTemplate.SetValue("parentid", entry.Base.ParentTaskId);
            _logTemplate.SetValue("id", entry.Base.TaskId);
            _logTemplate.SetValue("message", entry.LogMessage);
            _logTemplate.SetValue("exception", GetExceptionString(entry.Base.Exception));
            _logTemplate.SetValue("module", entry.Base.Module); 
            return _logTemplate.ToString();
        }

        /// <summary>
        /// Returns a String Representation of an Exception
        /// </summary>
        /// <param name="ex">Exception to be returned</param>
        /// <returns>string representation of exception. null if exception is null</returns>
        protected virtual string GetExceptionString(Exception ex)
        {
            if (ex == null) return null;

            var stackTraces = new List<string>();
            var exceptionLine = new StringBuilder();
            var stackTraceDelimiter = $"{Environment.NewLine}   --- End of inner exception stack trace ---{Environment.NewLine}";

            var currentEx = ex;
            while (currentEx != null)
            {
                if (exceptionLine.Length != 0)
                {
                    exceptionLine.Append(" ---> ");
                }
                exceptionLine.AppendFormat("{0}: {1}", currentEx.GetType(), currentEx.Message);

                if (!string.IsNullOrEmpty(currentEx.StackTrace))
                {
                    stackTraces.Add(currentEx.StackTrace);
                }

                currentEx = currentEx.InnerException;
            }

            stackTraces.Reverse();

            return exceptionLine + Environment.NewLine + string.Join(stackTraceDelimiter, stackTraces) + Environment.NewLine;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Initializes the Appender
        /// </summary>
        /// <param name="config">Config used for initialization</param>
        public virtual void Initialize(AppenderConfiguration config, LogSubject logSubject)
        {
            _config = config;
            _subject = logSubject; 
            InitLogTemplate(config.EntryFormat);
        }

        /// <summary>
        /// This is called when a new Log Entry is appended
        /// </summary>
        /// <param name="entry">LogEntry to Append</param>
        public abstract void Append(LogEntryExt entry);
        #endregion
    }
}


