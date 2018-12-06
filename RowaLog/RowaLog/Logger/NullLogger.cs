using System;
using Rowa.Lib.Log.Types;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// The default logger to use when no logging framework is available.
    /// </summary>
    internal class NullLogger : ILog
    {
        /// <summary>
        /// Appends the given LogEntry to the LogQueue in Order to be logged
        /// </summary>
        /// <param name="entry"></param>
        public void Append(LogEntry entry)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Debug(string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Debug(ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Debug(ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Debug(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Debug(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void UserIn(string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void UserIn(ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void UserIn(ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void UserIn(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void UserIn(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void ExtIf(string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void ExtIf(ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void ExtIf(ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void ExtIf(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void ExtIf(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Info(string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Info(ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Info(ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Info(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Info(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Warning(string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Warning(ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Warning(ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Warning(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Warning(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(Exception exception, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(Exception exception, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(Exception exception, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Error(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(Exception exception, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(Exception exception, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(Exception exception, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Fatal(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        #region Audit

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(Exception exception, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="jsonString">The string to append to the log entry.</param>
        public void Audit(string jsonString)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }


        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(Exception exception, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(Exception exception, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public void Audit(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            //Dummy Implementation
        }
        #endregion Audit
    }
}
