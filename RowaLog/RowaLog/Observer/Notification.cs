using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Observer
{
    internal class Notification
    {

        #region ------------- Fields -------------
        private readonly LogEventType _type;

        private readonly EventArgs _args; 
        #endregion

        #region ------------- Konstruktor -------------
        /// <summary>
        /// Creates a new Notification Object containing the given Notification Type 
        /// And the provided Event Args
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        public Notification(LogEventType type, EventArgs args)
        {
            _type = type;
            _args = args; 
        }
        #endregion

        #region ------------- Methods -------------

        /// <summary>
        /// The LogEventType this Notification is used of 
        /// Important in Order to Parse the EventArgs
        /// </summary>
        internal LogEventType Type => _type;

        /// <summary>
        /// The Arguments the Notification contains 
        /// </summary>
        internal EventArgs Args => _args; 

        #endregion
    }
}
