using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Configuration.Factory
{
    internal class AppenderFormat
    {
        #region ------------- Member -------------

        internal string EntryFormat { get; set; }

        internal string HeaderType { get; set; }

        internal string HeaderVersion { get; set; }

        internal string HeaderFormat { get; set; }

        internal LogConfigurationCreatorBase ConfigurationCreator { get; set; }
        #endregion

        #region ------------- Methods -------------
        /// <summary>
        /// Returns the String that should be written as a Header 
        /// </summary>
        public string GetHeaderString()
        {
            //Replace the Values in the HeaderFormat with the given ones or return empty
            LogConfigTemplate headerTemplate = new LogConfigTemplate(HeaderFormat);
            if (!string.IsNullOrEmpty(this.HeaderType) && !string.IsNullOrEmpty(this.HeaderVersion))
            {
                headerTemplate.SetValue("headerType", this.HeaderType);
                headerTemplate.SetValue("headerVersion", this.HeaderVersion);
                headerTemplate.SetValue("newLine", Environment.NewLine);
                return headerTemplate.ToString();
            }
            return string.Empty;
        }
            
        #endregion

    }
}
