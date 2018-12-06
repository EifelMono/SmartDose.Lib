using Rowa.Lib.Log.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Class that is used to Write Diagnostic Messages 
    /// for RowaLog in order to be able to comprehend the
    /// behavoir of RowaLog 
    /// </summary>
    internal class LogDiagnosticsWriter
    {
        #region ------------- Fields -------------

        /// <summary>
        /// The Name of the Event Source that should be 
        /// used for the Event Log 
        /// </summary>
        private string _eventSourceName; 

        /// <summary>
        /// The ID that will be used for this Disgnostic Session
        /// </summary>
        private int _diagnosticID; 
        #endregion

        #region ------------- Konstruktor -------------
        /// <summary>
        /// Creates a new LogDiagnostics Writer with the given product and component
        /// </summary>
        /// <param name="product"></param>
        /// <param name="component"></param>
        public LogDiagnosticsWriter(string product, string component)
        {
            Initialize(Globals.Constants.EventViewerSourceName); 

            //Write Startup Message
            WriteEntry(String.Format("RowaLog version {0} Instance with product {1} and component {2} will have ID {3}",
                                      GetCurrentVersion(), product, component, _diagnosticID));
        }

        /// <summary>
        /// Creates a new LogDiagnosticsWriter using the given Name as a Source 
        /// </summary>
        /// <param name="name"></param>
        public LogDiagnosticsWriter(string name)
        {
            Initialize(name);

            //Startup Message
            WriteEntry(String.Format("RowaLog version {0} {1} will have ID {2}", GetCurrentVersion(), name, _diagnosticID)); 
        }

        #endregion

        #region ------------- Methods -------------

        /// <summary>
        /// Initializes the LogDiagnosticsWriter that will create a Source with the 
        /// given Name 
        /// </summary>
        /// <param name="sourceName"></param>
        private void Initialize(string sourceName)
        {
            _eventSourceName = sourceName; 

            //Get ID 
            _diagnosticID = GetCurrentSessionID();
        }

        /// <summary>
        /// Trys to get the Id for the current Process, 
        /// returns -1 when not possible 
        /// </summary>
        /// <returns></returns>
        private int GetCurrentSessionID()
        {
            int res = -1; 

            try
            {
                using (var p = Process.GetCurrentProcess())
                {
                    res = p.Id;
                }
            }
            catch
            {
                //If it does not work we just return -1
            }
            return res; 
        }


        /// <summary>
        /// Returns the current Version of RowaLog 
        /// </summary>
        /// <returns></returns>
        private string GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Writes the given Message as a Entry to the Windows Event Log 
        /// </summary>
        /// <param name="message"></param>
        internal void WriteEntry(string message)
        {
            Trace.WriteLine("RowaLog Information: " + message);
        }

        /// <summary>
        /// Writes a Error to the Windows Event Log 
        /// </summary>
        /// <param name="message"></param>
        internal void WriteError(string message)
        {
            Trace.WriteLine("RowaLog Error: " + message);
        }

        /// <summary>
        /// Writes a Warning to the Windows Event Log 
        /// </summary>
        /// <param name="message"></param>
        internal void WriteWarning(string message)
        {
            Trace.WriteLine("RowaLog Warning: " + message);
        }
        #endregion
    }
}
