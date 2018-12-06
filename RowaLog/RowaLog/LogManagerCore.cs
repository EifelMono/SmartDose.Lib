using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rowa.Lib.Log.Appender;
using Rowa.Lib.Log.Events;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Configuration;
using Rowa.Lib.Log.Configuration.Factory;
using System.Diagnostics;
using Rowa.Lib.Log.Observer;
using static Rowa.Lib.Log.LogCleanup;

namespace Rowa.Lib.Log
{
    public class LogManagerCore : IDisposable, IObserver
    {
        #region Members
        /// <summary>
        /// Holds the current log configuration.
        /// </summary>
        private LogConfiguration _configuration = null;

        /// <summary>
        /// Class wide locking object
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Configuration for Developer Logs
        /// </summary>
        private LogConfiguration _developerConfiguration = null;

        /// <summary>
        /// Configuration for PLC Logs 
        /// </summary>
        private LogConfiguration _PLCConfiguration = null; 

        /// <summary>
        /// Manager for the Filestream used for Logging 
        /// DisposableSigleton to be shared from multiple Threads
        /// </summary>
        private LogFileStreamManager _fileStreamManager;

        /// <summary>
        /// Writer that is used to Write Rowa Log Disgnostic Messages to the 
        /// Windows Event Viewer
        /// </summary>
        private LogDiagnosticsWriter _logDiagnosticsWriter; 

        /// <summary>
        /// Holds the currently active log cleanup instance.
        /// </summary>
        private LogCleanup _logCleanup = null;

        /// <summary>
        /// Dictionary of cached logger instances.
        /// </summary>
        private readonly Dictionary<string, ILog> _loggers = new Dictionary<string, ILog>();

        /// <summary>
        /// Dictionary of cached WWI logger instances.
        /// </summary>
        private readonly Dictionary<string, WwiLogger> _wwis = new Dictionary<string, WwiLogger>();

        /// <summary>
        /// Observer Subject that is used for all internal Notifications 
        /// </summary>
        private LogSubject _logSubject; 

        /// <summary>
        /// If true then the Null Logger is used. This is for testing purposes
        /// </summary>
        private readonly bool _useNullLogger = false;

        /// <summary>
        /// Identicates which Component this LogManager is used for 
        /// </summary>
        private readonly string _component;

        /// <summary>
        /// Identicates which Product this LogManager is used for 
        /// </summary>
        private readonly string _product;

        /// <summary>
        /// The Product that is set for the current LogManager
        /// </summary>
        public string Product => _product;

        /// <summary>
        /// The Component that is set for the current LogManager
        /// </summary>
        public string Component => _component; 

        /// <summary>
        /// Event that is thrown when an Error occurs in the Logger
        /// </summary>
        public event OnNewError OnError;

        /// <summary>
        /// NullLogger that is used to be returned as nulllogger 
        /// </summary>
        private readonly NullLogger _nullLogger;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the default log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        public LogManagerCore(string product, string component) : this(product, component, "RowaLogConfig.xml")
        {
        }

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the default log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        /// <param name="useConsole">Disable Use of Console logging for non-Console Applications</param>
        public LogManagerCore(string product, string component, bool useConsole) : this(product, component, "RowaLogConfig.xml", string.Empty, useConsole)
        {
        }

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the specified log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        /// <param name="logConfigFile"> The name of the log configuration file to use. </param>
        public LogManagerCore(string product, string component, string logConfigFile) : this(product, component, logConfigFile, string.Empty)
        {
        }

        /// <summary>
        /// Initializes the logging engine with the specified 
        /// environment properties and the specified log configuration file.
        /// </summary>
        /// <param name="product"> The product name to set as environment setting. </param>
        /// <param name="component"> The product component name to set as environment setting. </param>
        /// <param name="logConfigFile"> The name of the log configuration file to use. </param>
        /// <param name="componentId"> The Identifier for the Component. </param>
        public LogManagerCore(string product, string component, string logConfigFile, string componentId) : this(product, component, logConfigFile, componentId, true)
        {
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
        public LogManagerCore(string product, string component, string logConfigFile, string componentId, bool useConsole) : this(product, component, logConfigFile, componentId, useConsole, true)
        {
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
        public LogManagerCore(string product, string component, string logConfigFile, string componentId, bool useConsole, bool initCleanup)
        {
            if (string.IsNullOrEmpty(product))
            {
                throw new ArgumentException("Invalid product specified!");
            }

            if (string.IsNullOrEmpty(component))
            {
                throw new ArgumentException("Invalid component specified!");
            }

            //Set the Internal properties
            _component = component;
            _product = product;

            ConfigureLogging(product, component, useConsole);
            LoadLogManager(product, component, initCleanup);
        }

        /// <summary>
        /// Initializes the Logger to use Null Output logging
        /// </summary>
        public LogManagerCore()
        {
            _product = String.Empty;
            _component = String.Empty; 
            _useNullLogger = true;
            _nullLogger = new NullLogger();
        }

        #endregion Constructors

        #region Propeties
        /// <summary>
        /// Returns the Base Directory of Rowa Log 
        /// </summary>
        public string BaseLogDirectory => Globals.Constants.LogBaseDirectory; 

        #endregion

        #region Public methods                
        /// <summary>
        /// Initializes the Cleanup Thread in case the Cleanup was not Initialized at startup.
        /// </summary>
        public void InitCleanup()
        {
            if (disposedValue) return;

            _logCleanup.Initialize();
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
        public ILog GetLogger(string name)
        {
            return GetLogger(_configuration, name);
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
        public ILog GetLogger(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException("Invalid type specified.");
            }

            return GetLogger(type.FullName);
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
        public ILog GetDeveloperLogger(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name specified.");
            }

            name = $"DeveloperLog.{name}";

            return GetLogger(_developerConfiguration, name);
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
        public ILog GetPLCLogger(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name specified.");
            }

            return GetLogger(_PLCConfiguration, name);
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
        public ILog GetDeveloperLogger(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException("Invalid type specified.");
            }

            return GetLogger($"DeveloperLog.{type.Name}");
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
        public ILog GetAuditLogger(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name specified.");
            }

            return GetLogger($"AuditLog.{name}");
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
        public ILog GetAuditLogger(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException("Invalid type specified.");
            }

            return GetLogger($"AuditLog.{type.Name}");
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
        public IWwi GetWwi(string description, string remoteAddress, ushort port)
        {
            lock (_lock)
            {
                if (disposedValue) return new NullWwiLogger();

                if (string.IsNullOrEmpty(description))
                {
                    throw new ArgumentException("Invalid description specified.");
                }

                if (string.IsNullOrEmpty(remoteAddress))
                {
                    throw new ArgumentException("Invalid remoteAddress specified.");
                }

                if (port == 0)
                {
                    throw new ArgumentException("Invalid port specified.");
                }

                if (string.IsNullOrEmpty(_configuration.LogDirectory))
                {
                    return new NullWwiLogger();
                }

                var wwiName = $"{description}-{remoteAddress}-{port}";

                if (_wwis.ContainsKey(wwiName))
                {
                    return _wwis[wwiName];
                }

                _wwis[wwiName] = new WwiLogger(_fileStreamManager, _configuration.LogDirectory, description, remoteAddress, port, _logSubject);
                return _wwis[wwiName];
            }
        }        
        #endregion

        #region Private methods
        /// <summary>
        /// This Method is used to throw logger error events
        /// </summary>
        /// <param name="args">Logger Error Data</param>
        private void ExecuteLoggerError(LogErrorEventArgs args)
        {
            if (args == null) return;

            WriteDiagnosticsError("An Log Error occured. Appender Name: " + args.AppenderName +
                                  " LoggerName: " + args.LoggerName +
                                  " Error: " + args.Error +
                                  " Method: " + args.Method +
                                  " Message: " + args.Message);

            OnError?.Invoke(args);
        }

        /// <summary>
        /// This Method is used to process LoggerIOEvent Notifications
        /// </summary>
        /// <param name="notification"></param>
        private void ExecuteLoggerIO(LogIOEventArgs notification)
        {
            //Check wheter there is a old File that should be ziped
            if (notification.Type == LoggerIOEventType.NewLogFileCreated && !String.IsNullOrEmpty(notification.OldFileName))
            {
                WriteDiagnosticsEntry("LogFile should be compressed from Cleanup Thread: " + notification.OldFileName);
                _logCleanup.CompressFileAsync(notification.OldFileName, _configuration);

            }
        }

        /// <summary>
        /// Returns a Logger with the given name
        /// </summary>
        /// <param name="config">configuration for the Logger to be used</param>
        /// <param name="name">name of the logger</param>
        /// <returns>logger instance</returns>
        private ILog GetLogger(LogConfiguration config, string name)
        {
            lock (_lock)
            {
                if (disposedValue) return new NullLogger();

                if (_useNullLogger) return _nullLogger;

                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Invalid name specified.");
                }

                if (config == null)
                {
                    throw new ArgumentException("Invalid configuration specified.");
                }

            
                if (_loggers.ContainsKey(name))
                {
                    return _loggers[name];
                }

                var logger = new Logger(config, _logSubject, name);

                _loggers[name] = logger;
                return logger;
            }
        }

        /// <summary>
        /// Handles when a Fatal Error occured in the Cleanup Part of the Application 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="product"></param>
        /// <param name="component"></param>
        private void HandleFatalCleanupError(string message, string product, string component)
        {
            if (product == this.Product && component == this.Component)
            {
                ILog logger = GetLogger(Globals.Constants.CleanupFatalErrorLogName);

                logger.Fatal(message);
            }
        }

        /// <summary>
        /// Writes a Message to the Diagnostics Writer 
        /// </summary>
        /// <param name="message"></param>
        private void WriteDiagnosticsEntry(string message)
        {
            if (_logDiagnosticsWriter != null)
            {
                _logDiagnosticsWriter.WriteEntry(message);
            }
        }

        /// <summary>
        /// Writes a Error Message to the Diagnostic Writer 
        /// </summary>
        /// <param name="message"></param>
        private void WriteDiagnosticsError(string message)
        {
            if (_logDiagnosticsWriter != null)
            {
                _logDiagnosticsWriter.WriteError(message);
            }
        }

        /// <summary>
        /// Configures the underlying logging implementation with the specified configuration file.
        /// </summary>
        /// <param name="product">
        /// The product name to set as environment setting.
        /// </param>
        /// <param name="component">
        /// The product component name to set as environment setting.
        /// </param>
        /// <param name="useConsole">Disable Use of Console logging for non-Console Applications</param>
        private void ConfigureLogging(string product, string component, bool useConsole)
        {
            //Get the LogConfigurations from the Factory
            _configuration = LogConfigurationFactory.GetLogConfiguration(ConfigurationType.Default, product, component);
            _developerConfiguration = LogConfigurationFactory.GetLogConfiguration(ConfigurationType.Developer, product, component);
            _PLCConfiguration = LogConfigurationFactory.GetLogConfiguration(ConfigurationType.PLC, product, component);

            if (!useConsole)
            {
                _configuration.AppenderConfigurations.RemoveAll(
                    x => x.AppenderType == typeof(LogAppenderConsole) || x.AppenderType == typeof(LogAppenderConsoleAsync));

                _developerConfiguration.AppenderConfigurations.RemoveAll(
                    x => x.AppenderType == typeof(LogAppenderConsole) || x.AppenderType == typeof(LogAppenderConsoleAsync));
            }
        }

        /// <summary>
        /// Loads the log manager of the underlying logging implementation.
        /// </summary>
        private void LoadLogManager(string product, string component, bool initCleanup)
        {
            _logDiagnosticsWriter = new LogDiagnosticsWriter(product, component);

            _logCleanup = ServiceLocator.Get<LogCleanup>();
            _logCleanup.AddConfig(_configuration);
            _logCleanup.LogCleanupFatalError += HandleFatalCleanupError;
            if (initCleanup)
            {
                InitCleanup();
            }


            _logSubject = new LogSubject();
            _logSubject.SubscribeObserver(this);

            _fileStreamManager = ServiceLocator.Get<LogFileStreamManager>();
            _fileStreamManager.Initialize(Globals.Constants.LogEncoding, _logSubject, Globals.Constants.MaxFileOpenTimeMs);
        }

        /// <summary>
        /// Notification Method for the Implementation of the IObserver Interface, 
        /// Notifies all Important events 
        /// </summary>
        /// <param name="notification"></param>
        void IObserver.onNotify(Notification notification)
        {
            if (notification.Type == LogEventType.Error)
            {
                ExecuteLoggerError((LogErrorEventArgs)notification.Args);
            }
            if (notification.Type == LogEventType.LogIO)
            {
                ExecuteLoggerIO((LogIOEventArgs)notification.Args);
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                lock(_lock)
                {
                    if (!disposedValue && disposing)
                    {
                        WriteDiagnosticsEntry("LogManagerCore: Dispose was called!");
                        WriteDiagnosticsEntry("Dispose Loggers");

                        foreach (var logger in _loggers)
                        {
                            IDisposable disposablelogger = logger.Value as IDisposable;

                            disposablelogger?.Dispose();
                        }
                        _loggers.Clear();

                        WriteDiagnosticsEntry("Dispose wwis");

                        foreach (var wwi in _wwis)
                        {
                            IDisposable disposablelogger = wwi.Value as IDisposable;

                            disposablelogger?.Dispose();
                        }
                        _wwis.Clear();

                        WriteDiagnosticsEntry("Dispose Log Cleanup");
                        //Is Null in case of Null Logger 
                        if (_logCleanup != null)
                        {
                            _logCleanup.RemoveConfig(_configuration.Product, _configuration.Component);
                            ServiceLocator.DisposeObject(ref _logCleanup);
                        }

                        OnError -= ExecuteLoggerError;
                        WriteDiagnosticsEntry("Dispose FileStream Manager");

                        ServiceLocator.DisposeObject(ref _fileStreamManager);

                        WriteDiagnosticsEntry("Dispose Observer");
                        if (_logSubject != null)
                        {
                            _logSubject.UnsubscribeObserver(this);
                            _logSubject.Dispose();
                            _logSubject = null;
                        }
                    }


                    disposedValue = true;
                }
            }
        }



        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        
        #endregion
    }
}
