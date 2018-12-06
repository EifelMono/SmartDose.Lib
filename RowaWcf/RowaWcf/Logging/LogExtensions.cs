using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Wcf.Logging
{
    /// <summary>
    /// Class which extends any managed type with log methods to 
    /// ease the usage of the logging functionality.
    /// </summary>
    internal static class LogExtensions
    {
        #region Members

        private static readonly DynamicMethod DebugMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.ILog", "Debug", typeof(string), typeof(object[]));
        private static readonly DynamicMethod InfoMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.ILog", "Info", typeof(string), typeof(object[]));
        private static readonly DynamicMethod WarningMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.ILog", "Warning", typeof(string), typeof(object[]));
        private static readonly DynamicMethod ErrorMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.ILog", "Error", typeof(string), typeof(object[]));
        private static readonly DynamicMethod ErrorExMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.ILog", "Error", typeof(Exception), typeof(string), typeof(object[]));
        private static readonly DynamicMethod FatalMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.ILog", "Fatal", typeof(string), typeof(object[]));
        private static readonly DynamicMethod FatalExMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.ILog", "Fatal", typeof(Exception), typeof(string), typeof(object[]));

        #endregion

        #region Methods

        /// <summary>
        /// Writes a debug log entry.
        /// </summary>
        /// <param name="instance">The object instance which writes the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Debug(this object instance, string format, params object[] args)
        {
            DebugMethod.Invoke(LogManager.GetLogger(instance.GetType()), format, args);
        }

        /// <summary>
        /// Writes an informational log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Info(this object instance, string format, params object[] args)
        {
            InfoMethod.Invoke(LogManager.GetLogger(instance.GetType()), format, args);
        }

        /// <summary>
        /// Writes a warning log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Warning(this object instance, string format, params object[] args)
        {
            WarningMethod.Invoke(LogManager.GetLogger(instance.GetType()), format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, string format, params object[] args)
        {
            ErrorMethod.Invoke(LogManager.GetLogger(instance.GetType()), format, args);
        }

        /// <summary>
        /// Writes an error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Error(this object instance, Exception exception, string format, params object[] args)
        {
            ErrorExMethod.Invoke(LogManager.GetLogger(instance.GetType()), exception, format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, string format, params object[] args)
        {
            FatalMethod.Invoke(LogManager.GetLogger(instance.GetType()), format, args);
        }

        /// <summary>
        /// Writes a fatal error log entry.
        /// </summary>
        /// <param name="exception">The exception to append to the log entry.</param>
        /// <param name="format">The format string of the log entry.</param>
        /// <param name="args">The format arguments of the log entry.</param>
        public static void Fatal(this object instance, Exception exception, string format, params object[] args)
        {
            FatalExMethod.Invoke(LogManager.GetLogger(instance.GetType()), exception, format, args);
        }


        #endregion
    }
}
