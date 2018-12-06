using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using Rowa.Lib.Log.Appender;
using Rowa.Lib.Log.Events;
using Rowa.Lib.Log.Types;

[assembly: InternalsVisibleTo("RowaLogUnitTests, PublicKey=" 
                              + "0024000004800000940000000602000000240000525341310004000001000100dd9037bde986fc"
                              + "1303cbec7a1d6526330634cd028cc75e9bb9f5dacee20973f21b78e14c1909cc43ff52896635d0"
                              + "83f82739ffe05a4d38d95742a2d73d0abbd3421d2a2ffffb976e7906e4afbc74a1f9edd68837ed"
                              + "2577bde70ec1225a0f4d358f03b90d25d4f1c7aef909b44db72a8816e7bfff13be5a67bf77934c"
                              + "3e540cd2")]

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey="
                              + "0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99"
                              + "c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654"
                              + "753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46"
                              + "ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484c"
                              + "f7045cc7")]

namespace Rowa.Lib.Log
{
    /// <summary>
    /// The log manager is the centralized entry point for the logging interface.
    /// </summary>
    public static class LogManager
    {
        #region Members
        /// <summary>
        /// The LogManagerCore that is used for Logging -> 
        /// This LogManager is static wrapper for the non static CoreManager 
        /// </summary>
        private static LogManagerCore _coreManager; 

        /// <summary>
        /// Event that is thrown when an Error occurs in the Logger
        /// </summary>
        public static event OnNewError OnError;

        /// <summary>
        /// Specifies wheter the class has been Initialized before 
        /// </summary>
        private static bool _intialized; 
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the default log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        public static void Initialize(string product, string component)
        {
            Initialize(product, component, "RowaLogConfig.xml");
        }

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the default log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        /// <param name="useConsole">Disable Use of Console logging for non-Console Applications</param>
        public static void Initialize(string product, string component, bool useConsole)
        {
            Initialize(product, component, "RowaLogConfig.xml", string.Empty, useConsole, true);
        }

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the default log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        /// <param name="useConsole">Disable Use of Console logging for non-Console Applications</param>
        public static void Initialize(string product, string component, bool useConsole, bool initCleanup)
        {
            Initialize(product, component, "RowaLogConfig.xml", string.Empty, useConsole, initCleanup);
        }

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the specified log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        /// <param name="logConfigFile"> The name of the log configuration file to use. </param>
        public static void Initialize(string product, string component, string logConfigFile)
        {
            Initialize(product,component,logConfigFile, string.Empty);
        }

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the specified log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        /// <param name="logConfigFile"> The name of the log configuration file to use. </param>
        /// <param name="componentId"> The Identifier for the Component. </param>
        public static void Initialize(string product, string component, string logConfigFile, string componentId)
        {
            Initialize(product, component, logConfigFile, componentId, true, true);
        }

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the specified log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        /// <param name="logConfigFile"> The name of the log configuration file to use. </param>
        /// <param name="componentId"> The Identifier for the Component. </param>
        /// /// <param name="useConsole">Disable Use of Console logging for non-Console Applications</param>
        public static void Initialize(string product, string component, string logConfigFile, string componentId, bool useConsole, bool initCleanup)
        {
            if (_intialized) return; 

            //Initializes the CoreLogManager and Abonent the Event 
            _coreManager = new LogManagerCore(product, component, logConfigFile, componentId, useConsole, initCleanup);
            _coreManager.OnError += ExecuteLoggerError;
            _intialized = true; 
        }

        /// <summary>
        /// Initializes the Logger to use Null Output logging
        /// </summary>
        public static void InitializeNull()
        {
            if (_intialized) return;
            
            _coreManager = new LogManagerCore();
            _coreManager.OnError += ExecuteLoggerError;
            _intialized = true; 
        }

        /// <summary>
        /// Initializes the Cleanup Thread in case the Cleanup was not Initialized at startup.
        /// </summary>
        public static void InitCleanup()
        {
            _coreManager?.InitCleanup();
        }
        #endregion Constructors

        #region Propeties
        /// <summary>
        /// Returns the Base Directory of Rowa Log 
        /// </summary>
        public static string BaseLogDirectory => _coreManager.BaseLogDirectory; 

        #endregion

        #region Methods
        /// <summary>
        /// This Method is used to throw logger error events
        /// </summary>
        /// <param name="args">Logger Error Data</param>
        private static void ExecuteLoggerError(LogErrorEventArgs args)
        {
            if (args == null) return;

            OnError?.Invoke(args);
        }

        /// <summary>
        /// Gets or creates the logger object with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the logger to get.
        /// </param>
        /// <returns>
        /// According logger instance with the specified name.
        /// </returns>
        public static ILog GetLogger(string name)
        {
            return _coreManager.GetLogger(name); 
        }

        /// <summary>
        /// Gets or creates the logger object for the specified type.
        /// </summary>
        /// <param name="type">
        /// The type to get the logger for.
        /// </param>
        /// <returns>
        /// According logger instance for the specified type.
        /// </returns>
        public static ILog GetLogger(Type type)
        {
            return _coreManager.GetLogger(type); 
        }

        /// <summary>
        /// Gets or creates the developer logger object with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the developer logger to get.
        /// </param>
        /// <returns>
        /// According developer logger instance with the specified name.
        /// </returns>
        public static ILog GetDeveloperLogger(string name)
        {
            return _coreManager.GetDeveloperLogger(name); 
        }

        /// <summary>
        /// Returns a Logger that could be used for PLC Logs
        /// PLC Logs will be written with a special Header Format 
        /// -> LogViewer is able to display them differently
        /// </summary>
        /// <param name="name">
        /// The name of the PLC logger to get.
        /// </param>
        /// <returns>
        /// According PLC logger instance with the specified name.
        /// </returns>
        public static ILog GetPLCLogger(string name)
        {
            return _coreManager.GetPLCLogger(name); 
        }

        /// <summary>
        /// Gets or creates the developer logger object for the specified type.
        /// </summary>
        /// <param name="type">
        /// The type to get the developer logger for.
        /// </param>
        /// <returns>
        /// According developer logger instance for the specified type.
        /// </returns>
        public static ILog GetDeveloperLogger(Type type)
        {
            return _coreManager.GetDeveloperLogger(type); 
        }

        /// <summary>
        /// Gets or creates the Audit logger object with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the Audit logger to get.
        /// </param>
        /// <returns>
        /// According Audit logger instance with the specified name.
        /// </returns>
        public static ILog GetAuditLogger(string name)
        {
            return _coreManager.GetAuditLogger(name); 
        }

        /// <summary>
        /// Gets or creates the Audit logger object for the specified type.
        /// </summary>
        /// <param name="type">
        /// The type to get the Audit logger for.
        /// </param>
        /// <returns>
        /// According Audit logger instance for the specified type.
        /// </returns>
        public static ILog GetAuditLogger(Type type)
        {
            return _coreManager.GetAuditLogger(type); 
        }

        /// <summary>
        /// Creates a WWI file logger with the specified properties.
        /// </summary>
        /// <param name="description">The description of the logged communication.</param>
        /// <param name="remoteAddress">The remote address of the communication partner.</param>
        /// <param name="port">The port which is used for the communication.</param>
        /// <returns>
        /// According WWI file logger with the specified properties.
        /// </returns>
        public static IWwi GetWwi(string description, string remoteAddress, ushort port)
        {
            return _coreManager.GetWwi(description, remoteAddress, port); 
        }


        /// <summary>
        /// Processes any necessary cleanup actions of the logging engine.
        /// </summary>
        public static void Cleanup()
        {
            if (_intialized)
            {
                //Dispose the Core Manager
                _coreManager.OnError -= ExecuteLoggerError;
                _coreManager.Dispose();
                ServiceLocator.ForceDisposeAll();
                _intialized = false;
            }
        }
        #endregion
    }
}
