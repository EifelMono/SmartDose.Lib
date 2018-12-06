﻿using Rowa.Lib.Log.Appender;
using System;
using System.Collections.Generic;

namespace Rowa.Lib.Log.Configuration.Factory
{
    internal class LogConfigurationCreatorDeveloper : LogConfigurationCreatorBase
    {
        /// <summary>
        /// Creates a new LogConfiguration containing the Settings for 
        /// Developer Logs 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        internal override LogConfiguration Create(string product, string component,
                                                  string HeaderString, string EntryFormat)
        {
            LogConfiguration _config = new LogConfiguration(product, component);

            var logDirectory = new LogConfigTemplate(Globals.Constants.FilePath);

            logDirectory.SetValue("product", product);
            logDirectory.SetValue("component", component);

            _config.AppenderConfigurations = new List<AppenderConfiguration>()
            {
                 base.GetLogAppender<LogAppenderRollingFileAsync>("Developer", logDirectory, new List<AppenderLevelMapping>()
                {
                        new AppenderLevelMapping {Level = LogLevel.Warn},
                        new AppenderLevelMapping {Level = LogLevel.Error},
                        new AppenderLevelMapping {Level = LogLevel.Fatal},
                        new AppenderLevelMapping {Level = LogLevel.Userin},
                        new AppenderLevelMapping {Level = LogLevel.Audit},
                        new AppenderLevelMapping {Level = LogLevel.Debug},
                        new AppenderLevelMapping {Level = LogLevel.Extif},
                        new AppenderLevelMapping {Level = LogLevel.Info}
                }, HeaderString, EntryFormat),
                base.GetLogAppender<LogAppenderConsoleAsync>("Console", logDirectory, new List<AppenderLevelMapping>()
                {
                        new AppenderLevelMapping {Level = LogLevel.Error, ForeColor = ConsoleColor.White, BackColor = ConsoleColor.Red},
                        new AppenderLevelMapping {Level = LogLevel.Warn, ForeColor = ConsoleColor.Black, BackColor = ConsoleColor.Yellow},
                        new AppenderLevelMapping {Level = LogLevel.Fatal, ForeColor = ConsoleColor.Gray, BackColor = ConsoleColor.Black},
                        new AppenderLevelMapping {Level = LogLevel.Userin, ForeColor = ConsoleColor.Gray, BackColor = ConsoleColor.Black},
                        new AppenderLevelMapping {Level = LogLevel.Audit, ForeColor = ConsoleColor.White, BackColor = ConsoleColor.Blue},
                        new AppenderLevelMapping {Level = LogLevel.Debug, ForeColor = ConsoleColor.Gray, BackColor = ConsoleColor.Black},
                        new AppenderLevelMapping {Level = LogLevel.Extif, ForeColor = ConsoleColor.Gray, BackColor = ConsoleColor.Black},
                        new AppenderLevelMapping {Level = LogLevel.Info, ForeColor = ConsoleColor.Gray, BackColor = ConsoleColor.Black}
                }, HeaderString, EntryFormat)
            };

            return _config; 
        }
    }
}
