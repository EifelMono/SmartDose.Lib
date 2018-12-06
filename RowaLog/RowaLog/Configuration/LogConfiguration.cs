using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Rowa.Lib.Log.Appender;
using Rowa.Lib.Log.Configuration;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Class which is able to load and parse the log configuration file.
    /// </summary>
    internal class LogConfiguration : ICloneable
    {
        #region Properties

        /// <summary>
        /// Product this Log Configuration is configurated for
        /// </summary>
        public string Product { get; private set; }

        /// <summary>
        /// Component this Log Configuration is configurated for
        /// </summary>
        public string Component { get; private set; }

        /// <summary>
        /// Gets the directory were the log and wwi files are stored.
        /// </summary>
        public string LogDirectory { get; private set; }

        /// <summary>
        /// Gets the number of days to keep the WWI files.
        /// </summary>
        public uint KeepWwiFilesInDays { get; private set; }

        /// <summary>
        /// Gets the number of days to keep the log files.
        /// </summary>
        public uint KeepLogFilesInDays { get; private set; }

        /// <summary>
        /// Gets the number of days to keep the audit log files.
        /// </summary>
        public uint KeepAuditFilesInDays { get; private set; }

        /// <summary>
        /// Gets the flag wether old WWI files have to be compressed.
        /// </summary>
        public bool CompressOldWwiFiles { get; private set; }

        /// <summary>
        /// Gets the flag wether old log files have to be compressed.
        /// </summary>
        public bool CompressOldLogFiles { get; private set; }

        /// <summary>
        /// List of File Filters that are ignored during a Log CLeanup
        /// </summary>
        public List<string> CleanupIgnoreFilter { get; private set; }

        /// <summary>
        /// List of Appender Configurations to be used in this Config
        /// </summary>
        public List<AppenderConfiguration> AppenderConfigurations { get; set; }

        #endregion

        /// <param name="product">
        /// The product name to set as environment setting.
        /// </param>
        /// <param name="component">
        /// The product component name to set as environment setting.
        /// </param>
        public LogConfiguration(string product, string component)
        {
            if (string.IsNullOrEmpty(product))
            {
                throw new ArgumentException("Invalid product specified.");
            }

            if (string.IsNullOrEmpty(component))
            {
                throw new ArgumentException("Invalid component specified.");
            }

            this.Product = product;
            this.Component = component; 
            this.LogDirectory = Path.GetDirectoryName(Globals.Constants.FilePath);
            this.KeepWwiFilesInDays = Globals.Constants.KeepWwiFilesInDays;
            this.KeepLogFilesInDays = Globals.Constants.KeepLogFilesInDays;
            this.KeepAuditFilesInDays = Globals.Constants.KeepAuditFilesInDays;
            this.CompressOldWwiFiles = Globals.Constants.CompressOldWwiFiles;
            this.CompressOldLogFiles = Globals.Constants.CompressOldLogFiles;
            this.AppenderConfigurations = new List<AppenderConfiguration>(); 
            CleanupIgnoreFilter = new List<string>(Globals.Constants.CleanupIgnoreFilter);

            this.LogDirectory = new LogConfigTemplate(LogDirectory).SetValue("product", product).SetValue("component", component).ToString();
        }

        /// <summary>
        /// Returns a new Object that is a deep copy from the current object 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            LogConfiguration deepCopy = new LogConfiguration(this.Product, this.Component);

            //Deep Copy all Elements that are changeable from the outside
            deepCopy.CleanupIgnoreFilter.Clear();
            foreach (string filter in this.CleanupIgnoreFilter)
            {
                if (!String.IsNullOrEmpty(filter))
                {
                    deepCopy.CleanupIgnoreFilter.Add(String.Copy(filter));
                }
            }
            //Deep copy Appender Configuration 
            deepCopy.AppenderConfigurations = new List<AppenderConfiguration>();
            foreach (AppenderConfiguration config in this.AppenderConfigurations)
            {
                deepCopy.AppenderConfigurations.Add((AppenderConfiguration)config.Clone());
            }
            return deepCopy;
        }
    }
}
