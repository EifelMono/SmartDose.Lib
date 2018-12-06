using System;

namespace Rowa.Lib.Log.Extensions
{
    /// <summary>
    /// Class which extends any managed type with log methods to 
    /// ease the usage of the logging functionality.
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="instance">The object instance which writes the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Debug(this object instance, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Debug(format, args);
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Debug(this object instance, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Debug(taskId, format, args);
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Debug(this object instance, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Debug(taskId, format, args);
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Debug(this object instance, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Debug(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Debug(this object instance, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Debug(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void UserIn(this object instance, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).UserIn(format, args);
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void UserIn(this object instance, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).UserIn(taskId, format, args);
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void UserIn(this object instance, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).UserIn(taskId, format, args);
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void UserIn(this object instance, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).UserIn(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an user interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void UserIn(this object instance, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).UserIn(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void ExtIf(this object instance, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).ExtIf(format, args);
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void ExtIf(this object instance, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).ExtIf(taskId, format, args);
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void ExtIf(this object instance, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).ExtIf(taskId, format, args);
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void ExtIf(this object instance, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).ExtIf(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an external interface interaction log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void ExtIf(this object instance, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).ExtIf(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Info(this object instance, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Info(format, args);
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Info(this object instance, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Info(taskId, format, args);
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Info(this object instance, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Info(taskId, format, args);
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Info(this object instance, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Info(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Info(this object instance, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Info(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Warning(this object instance, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Warning(format, args);
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Warning(this object instance, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Warning(taskId, format, args);
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Warning(this object instance, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Warning(taskId, format, args);
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Warning(this object instance, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Warning(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Warning(this object instance, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Warning(parentTaskId, taskId, format, args);
        }

        #region Error

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(taskId, format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(taskId, format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, Exception exception, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(exception, format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, Exception exception, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(exception, taskId, format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, Exception exception, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(exception, taskId, format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(exception, parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Error(exception, parentTaskId, taskId, format, args);
        }

        #endregion Error

        #region Fatal

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(taskId, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(taskId, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, Exception exception, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, Exception exception, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, taskId, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, Exception exception, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, taskId, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, parentTaskId, taskId, format, args);
        }

        #endregion Fatal

        #region Audit

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="instance">The object instance which writes the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Audit(format, args);
        }

        /// <summary>
        /// Writes an Audit log entry.
        /// </summary>
        /// <param name="instance">The object instance which writes the log entry.</param>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, Exception exception, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Audit(exception, format, args);
        }

        /// <summary>
        /// Writes an Audit log from a json string.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="jsonString"></param>
        public static void Audit(this object instance, string jsonString)
        {
            LogManager.GetLogger(instance.GetType()).Audit(jsonString);
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(taskId, format, args);
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(taskId, format, args);
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a Audit log entry.
        /// </summary>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, Exception exception, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, taskId, format, args);
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, Exception exception, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, taskId, format, args);
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, Exception exception, ulong parentTaskId, ulong taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, parentTaskId, taskId, format, args);
        }

        /// <summary>
        /// Writes a Audit error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="parentTaskId">The parent task identifier which belongs to the log entry.</param>
        /// <param name="taskId">The task identifier which belongs to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Audit(this object instance, Exception exception, ITaskId parentTaskId, ITaskId taskId, string format, params object[] args)
        {
            LogManager.GetLogger(instance.GetType()).Fatal(exception, parentTaskId, taskId, format, args);
        }

        #endregion Audit

    }
}
