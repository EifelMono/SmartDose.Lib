using System;

namespace Rowa.Lib.Wcf.Logging
{
    /// <summary>
    /// Dynamic proxy for the RowaLog LogManager class.
    /// </summary>
    internal static class LogManager
    {
        #region Members

        private static readonly DynamicMethod GetLoggerByNameMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.LogManager", "GetLogger", typeof(string));
        private static readonly DynamicMethod GetLoggerByTypeMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.LogManager", "GetLogger", typeof(Type));
        private static readonly DynamicMethod GetWwiMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.LogManager", "GetWwi", typeof(string), typeof(string), typeof(ushort));
        private static readonly DynamicMethod LogMessageMethod = new DynamicMethod("RowaLog.dll", "Rowa.Lib.Log.IWwi", "LogMessage", typeof(string), typeof(bool));

        #endregion

        /// <summary>
        /// Gets or creates the logger object with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the logger to get.
        /// </param>
        /// <returns>
        /// According logger instance with the specified name.
        /// </returns>
        public static object GetLogger(string name)
        {
            return GetLoggerByNameMethod.Invoke(null, name);
        }

        /// <summary>
        /// Gets or creates the logger object for the specified type.
        /// </summary>
        /// <param name="type">
        /// The type to get the logger for.
        /// </param>
        /// <returns>
        /// According logger instance for the specified type.
        /// </returns>
        public static object GetLogger(Type type)
        {
            return GetLoggerByTypeMethod.Invoke(null, type);
        }

        /// <summary>
        /// Creates a WWI file logger with the specified properties.
        /// </summary>
        /// <param name="description">The description of the logged communication.</param>
        /// <param name="remoteAddress">The remote address of the communication partner.</param>
        /// <param name="port">The port which is used for the communication.</param>
        /// <returns>
        /// According WWI file logger with the specified properties.
        /// </returns>
        public static object GetWwi(string description, string remoteAddress, ushort port)
        {
            return GetWwiMethod.Invoke(null, description, remoteAddress, port);
        }

        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="wwi">The wwi log instance to use.</param>
        /// <param name="message">The message content to log.</param>
        /// <param name="isIncommingMessage">
        /// If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.
        /// </param>
        public static void LogMessage(object wwi, string message, bool isIncommingMessage = true)
        {
            LogMessageMethod.Invoke(wwi, message, isIncommingMessage);
        }
    }
}
