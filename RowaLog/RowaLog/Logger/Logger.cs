using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rowa.Lib.Log.Appender;
using Rowa.Lib.Log.Events;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Observer;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Default Logger Class that manages all Kinds of logging
    /// </summary>
    internal class Logger : ILog, IDisposable
    {
        #region Members
        /// <summary>
        /// delegate Definition for logging Event
        /// </summary>
        /// <param name="entry">Log Entry to be logged</param>
        private delegate void OnLog(LogEntryExt entry);

        /// <summary>
        /// Configuration of the Logger
        /// </summary>
        private readonly LogConfiguration _config;

        /// <summary>
        /// Subject used for Notifications
        /// </summary>
        private readonly LogSubject _subject; 

        /// <summary>
        /// Name of the Logger
        /// </summary>
        private readonly string _loggerName;

        /// <summary>
        /// List of all Appenders of the Logger
        /// </summary>
        private readonly List<ILogAppender> _appenders = new List<ILogAppender>();

        /// <summary>
        /// Dispatcher Events for Every Loglevel
        /// </summary>
        private readonly Dictionary<LogLevel, OnLog> _logDispatchers = new Dictionary<LogLevel, OnLog>();
        #endregion

        #region non-public Methods
        /// <summary>
        /// Appends a new Log Entry with All Appenders that have the Same Loglevel
        /// </summary>
        /// <param name="entry"></param>
        private void Append(LogEntryExt entry)
        {
            if (disposedValue || entry == null) return;
            if (_logDispatchers[entry.Base.Level] == null) return;

            _logDispatchers[entry.Base.Level](entry);
        }

        /// <summary>
        /// This Creates all Appenders from the Configuration
        /// </summary>
        private void CreateAppenders()
        {
            if (_config == null) return;

            var appenderFactory = new AppenderFactory();

            foreach (var configuration in _config.AppenderConfigurations)
            {
                configuration.LoggerName = _loggerName;
                var appender = appenderFactory.GetAppender(configuration, _subject);

                if (appender == null) continue;

                _appenders.Add(appender);

                foreach (var mapping in configuration.LevelMappings)
                {
                    _logDispatchers[mapping.Level] += appender.Append;
                }
            }
        }

        /// <summary>
        /// Creates Log Dispatching Events for all Log Levels
        /// </summary>
        private void CreateLogDispatchers()
        {
            foreach (var level in Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>())
            {
                _logDispatchers.Add(level, default(OnLog));
            }
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="config">Configuration of the Logger</param>
        /// <param name="loggerName">Name of the Logger</param>
        /// <param name="subject">The subject that should be used for Notifications</param>
        public Logger(LogConfiguration config, LogSubject subject, string loggerName)
        {
            _config = config;
            _loggerName = loggerName;
            _subject = subject; 

            CreateLogDispatchers();
            CreateAppenders();
        }
        #endregion


        #region ILog implementation
        /// <summary>
        /// Appends the given LogEntry to the LogQueue and Logs it 
        /// </summary>
        /// <param name="entry"></param>
        public void Append(LogEntry entry)
        {
            //Parse the given Entry to a LogEntryExt and provide the Entry
            Append(new LogEntryExt(entry, _subject)); 
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Debug(string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Debug,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Debug(ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Debug,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Debug(ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Debug,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Debug(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            ( 
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Debug,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Debug(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Debug,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void UserIn(string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Userin,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void UserIn(ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Userin,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void UserIn(ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Userin,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void UserIn(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Userin,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void UserIn(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Userin,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void ExtIf(string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Extif,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void ExtIf(ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Extif,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void ExtIf(ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Extif,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void ExtIf(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry{
                    Exception = null,
                    Level = LogLevel.Extif,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void ExtIf(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Extif,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Info(string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Info,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Info(ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Info,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Info(ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Info,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Info(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Info,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Info(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Info,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Warning(string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Warn,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Warning(ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Warn,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Warning(ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Warn,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Warning(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Warn,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Warning(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Warn,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Error,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Error,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Error,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Error,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Error,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        public virtual void Error(Exception exception, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Error,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(Exception exception, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Error,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(Exception exception, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Error,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Error,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Error(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Error,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Fatal,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Fatal,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Fatal,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Fatal,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Fatal,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(Exception exception, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Fatal,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(Exception exception, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Fatal,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(Exception exception, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Fatal,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Fatal,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Fatal(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            ( 
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Fatal,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Audit,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(Exception exception, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Audit,
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="jsonString">The string to append to the log entry.</param>
        public virtual void Audit(string jsonString)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Audit,
                    Format = jsonString,
                    Args = null
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                Level = LogLevel.Audit,
                TaskId = taskId.ToString(),
                Format = format,
                Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Audit,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Audit,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Audit,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(Exception exception, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = null,
                    Level = LogLevel.Audit,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(Exception exception, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Audit,
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Audit,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public virtual void Audit(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            Append(new LogEntryExt
            (
                new LogEntry
                {
                    Exception = exception,
                    Level = LogLevel.Audit,
                    ParentTaskId = parentTaskId.ToString(),
                    TaskId = taskId.ToString(),
                    Format = format,
                    Args = args
                },
                _subject
            ));
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
                    foreach (var appender in _appenders)
                    {
                        IDisposable disposableAppender =  appender as IDisposable;

                        disposableAppender?.Dispose();
                    }
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
