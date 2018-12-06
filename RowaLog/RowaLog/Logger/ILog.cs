using Rowa.Lib.Log.Types;
using System;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Interface which defines the methods of a Rowa compliant logger.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Appends the given LogEntry to the current LogQueue to be logged
        /// </summary>
        /// <param name="entry"></param>
        void Append(LogEntry entry); 

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Debug(string format, params object[] args);

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Debug(ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Debug(ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Debug(ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Debug(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void UserIn(string format, params object[] args);

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void UserIn(ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void UserIn(ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void UserIn(ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void UserIn(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void ExtIf(string format, params object[] args);

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void ExtIf(ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void ExtIf(ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void ExtIf(ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void ExtIf(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Info(string format, params object[] args);

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Info(ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Info(ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Info(ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Info(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Warning(string format, params object[] args);

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Warning(ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Warning(ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Warning(ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Warning(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(Exception exception, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(Exception exception, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(Exception exception, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Error(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(Exception exception, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(Exception exception, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(Exception exception, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Fatal(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        #region Audit

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(string format, params object[] args);

        /// <summary>
        /// Writes an Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(Exception exception, string format, params object[] args);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        void Audit(string jsonString);

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(Exception exception, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(Exception exception, ITaskId taskId, string format, params object[] args);

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args);

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        void Audit(Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args);

        #endregion Audit
    }
}
