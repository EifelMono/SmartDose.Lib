using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Appender
{
    /// <summary>
    /// This Class represents the Configuration of a Log Appender
    /// </summary>
    internal class AppenderConfiguration : ICloneable
    {
        /// <summary>
        /// Path and Filename of the Logfile
        /// </summary>
        public LogConfigTemplate FilePath { get; set; }

        /// <summary>
        /// Format String for Log Entries
        /// </summary>
        public LogConfigTemplate EntryFormat { get; set; }

        /// <summary>
        /// String that should be used as a File Header
        /// No Header will be set if the string is empty 
        /// </summary>
        public string HeaderString { get; set; }

        /// <summary>
        /// List Of Loglevels that are mapped to the Appender
        /// </summary>
        public List<AppenderLevelMapping> LevelMappings { get; set; }

        /// <summary>
        /// Maximum Size of Logfiles in Bytes
        /// </summary>
        public long MaxLogFileSizeInBytes { get; set; }

        /// <summary>
        /// Type of the Appender thats used by this config
        /// </summary>
        public Type AppenderType { get; set; }

        /// <summary>
        /// Name of the Appender
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the Logger
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// Constructor. What else?
        /// </summary>
        public AppenderConfiguration()
        {
            LevelMappings = new List<AppenderLevelMapping>();
        }

        /// <summary>
        /// Returns a new AppenderConfiguration object that is a Deep Copy of the caller object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            AppenderConfiguration config = new AppenderConfiguration();
            config.Name = Name != null ? String.Copy(this.Name) : "";
            config.LoggerName = LoggerName != null ? String.Copy(this.LoggerName) : "";
            config.AppenderType = AppenderType;
            config.MaxLogFileSizeInBytes = MaxLogFileSizeInBytes;
            config.EntryFormat = new LogConfigTemplate(this.EntryFormat);
            config.FilePath = new LogConfigTemplate(this.FilePath); 
            config.LevelMappings = new List<AppenderLevelMapping>(); 
            foreach(AppenderLevelMapping mapping in this.LevelMappings)
            {
                config.LevelMappings.Add((AppenderLevelMapping)mapping.Clone()); 
            }
            return config;
        }
    }
}
