using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Appender
{
    /// <summary>
    /// This Class represents a Mapping for a Loglevel in an Appender Configuration
    /// </summary>
    internal class AppenderLevelMapping : ICloneable
    {
        /// <summary>
        /// LogLevel that this Mapping Corresponds to
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// ForeColor for Log Entries (Mainly used in Console Appender)
        /// </summary>
        public ConsoleColor ForeColor { get; set; }

        /// <summary>
        /// Background Color for Log Entries (Mainly used in Console Appender)
        /// </summary>
        public ConsoleColor BackColor { get; set; }

        /// <summary>
        /// Returns a new object that is a deep copy of the current one 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new AppenderLevelMapping() { Level = this.Level, ForeColor = this.ForeColor, BackColor = this.BackColor }; 
        }
    }
}
