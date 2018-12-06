using Rowa.Lib.Log.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Configuration.Factory
{
    /// <summary>
    /// Base class that is used for 
    /// </summary>
    internal abstract class LogConfigurationCreatorBase
    {
        /// <summary>
        /// Creates a LogConfiguration for the type set with the class 
        /// </summary>
        /// <returns></returns>
        internal abstract LogConfiguration Create(string product, string component, string HeaderString, string EntryFormat);

        /// <summary>
        /// Returns a new Appender that uses the LogAppenderRollingFileAsync 
        /// and the provided LevelMapping 
        /// </summary>
        /// <param name="appender"></param>
        /// <param name="logDirectory"></param>
        /// <param name="levelMapping"></param>
        /// <returns></returns>
        protected AppenderConfiguration GetLogAppender<T>(string appender, LogConfigTemplate logDirectory,
                                                          List<AppenderLevelMapping> levelMapping, 
                                                          string HeaderString, string EntryFormat)
        {
            return new AppenderConfiguration
            {
                Name = appender,
                AppenderType = typeof(T),
                EntryFormat = new LogConfigTemplate(EntryFormat),
                HeaderString = HeaderString,
                FilePath = new LogConfigTemplate(logDirectory).SetValue("appender", appender),
                MaxLogFileSizeInBytes = Globals.Constants.MaxLogFileSizeInBytes,
                LevelMappings = levelMapping
            };
        }
    }
}
