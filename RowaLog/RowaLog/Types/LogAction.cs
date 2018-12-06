using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rowa.Lib.Log.Events;

namespace Rowa.Lib.Log.Types
{
    /// <summary>
    /// Represents one log Action
    /// </summary>
    internal class LogAction
    {
        /// <summary>
        /// Log Entry of the Action
        /// </summary>
        public LogEntryExt Entry { get; set; }

        /// <summary>
        /// The Actual Action to be performed
        /// </summary>
        public OnLogAction Action { get; set; }
    }
}
