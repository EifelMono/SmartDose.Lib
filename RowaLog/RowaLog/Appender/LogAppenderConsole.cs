using Rowa.Lib.Log.Types;
using System;

namespace Rowa.Lib.Log.Appender
{
    /// <summary>
    /// This Appender does Synchronous Console Logging
    /// </summary>
    internal class LogAppenderConsole : LogAppenderBase
    {
        #region Members
        /// <summary>
        /// Mutex for Thread Safety
        /// </summary>
        private static readonly object _mutex = new object();
        #endregion

        #region non-public Methods
        /// <summary>
        /// Returns the Foreground Color of a given Loglevel
        /// </summary>
        /// <param name="level">Loglevel used to get the Color</param>
        /// <returns>Color of Loglevel</returns>
        protected static ConsoleColor GetForeGroundColor(AppenderConfiguration config, LogLevel level)
        {
            var logLevelEntry = config.LevelMappings.Find(x => x.Level == level);

            if (logLevelEntry == null) return ConsoleColor.Gray;

            return logLevelEntry.ForeColor;
        }

        /// <summary>
        /// Returns the Background Color of a given Loglevel
        /// </summary>
        /// <param name="level">Loglevel used to get the Color</param>
        /// <returns>Color of Loglevel</returns>
        protected static ConsoleColor GetBackGroundColor(AppenderConfiguration config, LogLevel level)
        {
            var logLevelEntry = config.LevelMappings.Find(x => x.Level == level);

            if (logLevelEntry == null) return ConsoleColor.Black;

            return logLevelEntry.BackColor;
        }

        /// <summary>
        /// Writes a LogEntry to the Console
        /// </summary>
        /// <param name="entry">LogEntry to be written</param>
        protected bool WriteConsole(LogEntryExt entry)
        {
            //Return true, because this entry is not needed to be written
            if (entry == null) return true;

            lock (_mutex)
            {
                var logEntry = GetLogLine(entry);

                try
                {
                    Console.ForegroundColor = GetForeGroundColor(_config, entry.Base.Level);
                    Console.BackgroundColor = GetBackGroundColor(_config, entry.Base.Level);
                    Console.Write(logEntry);
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    ExecuteLoggerError(new LogErrorEventArgs {Error = LoggerError.LogConsoleOutputError, Message =  e.Message});
                    return false; 
                }
            }
            return true; 
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Writes a new Log Entry to the Console
        /// </summary>
        /// <param name="entry">LogEntry to Write</param>
        public override void Append(LogEntryExt entry)
        {
            WriteConsole(entry);
        }
        #endregion
    }
}
