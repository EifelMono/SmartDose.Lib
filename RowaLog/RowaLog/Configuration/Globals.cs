using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rowa.Lib.Log.Appender;

namespace Rowa.Lib.Log.Configuration
{
    internal class Globals 
    {
        #region Members

        protected static void InjectConstants(LogConstants constants)
        {
            _constants = constants; 
        }

        private static LogConstants _constants = new LogConstants();  

        /// <summary>
        /// Constants that can be used to request values 
        /// </summary>
        public static LogConstants Constants
        {
            get
            {
                return _constants; 
            }
        }
        
        #endregion
    }
}
