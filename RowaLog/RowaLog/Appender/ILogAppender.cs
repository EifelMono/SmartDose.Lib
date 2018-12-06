using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rowa.Lib.Log.Events;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Observer;

namespace Rowa.Lib.Log.Appender
{
    /// <summary>
    /// Interface for all Log Appenders to use inside Rowalog
    /// </summary>
    internal interface ILogAppender
    {
        /// <summary>
        /// Initializes the Log Appender
        /// </summary>
        /// <param name="config">Appender Configuration used for initialization</param>
        void Initialize(AppenderConfiguration config, LogSubject logSubject);

        /// <summary>
        /// Appends a new Log Entry to the Log
        /// </summary>
        /// <param name="entry">Log Entry to Append</param>
        void Append(LogEntryExt entry);
    }
}
