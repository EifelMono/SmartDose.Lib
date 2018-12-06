using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Rowa.Lib.Log.Configuration.Factory
{
    internal static class LogConfigurationFactory 
    {
        #region ------------- Fields -------------
        /// <summary>
        /// Dictionary of Creators that could be used to create Configurations
        /// </summary>
        private static Dictionary<ConfigurationType, AppenderFormat> _config;
        #endregion
        
        #region ------------- Konstruktor -------------
        static LogConfigurationFactory()
        {
            _config = new Dictionary<ConfigurationType, AppenderFormat>()
            {
                {
                    ConfigurationType.Default, new AppenderFormat()
                    {
                        ConfigurationCreator = new LogConfigurationCreatorDefault(),
                        EntryFormat = "{date[HH:mm:ss,fff]};{loglevel};{logger};{threadid};{parentid};{id};{message}{newline}{exception}",
                        HeaderType = "",
                        HeaderVersion = "",
                        HeaderFormat = Globals.Constants.HeaderFormat,
                    }
                },
                {
                    ConfigurationType.Developer, new AppenderFormat()
                    {
                        ConfigurationCreator = new LogConfigurationCreatorDeveloper(),
                        EntryFormat = "{date[HH:mm:ss,fff]};{loglevel};{logger};{threadid};{parentid};{id};{message}{newline}{exception}",
                        HeaderType = "",
                        HeaderVersion = "",
                        HeaderFormat = Globals.Constants.HeaderFormat, 
                    }
                },
                {
                    ConfigurationType.PLC, new AppenderFormat()
                    {
                        ConfigurationCreator = new LogConfigurationCreatorDefault(),
                        EntryFormat = "{date[HH:mm:ss,fff]};{loglevel};{logger};{module};{threadid};{parentid};{id};{message}{newline}{exception}",
                        HeaderType = "PLC",
                        HeaderVersion = "1.0",
                        HeaderFormat = Globals.Constants.HeaderFormat,
                    }
                },
            };
        }
        #endregion

        #region ------------- Methods -------------
        /// <summary>
        /// Returns the LogConfiguration for the given type, product and component
        /// </summary>
        /// <param name="type"></param>
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        internal static LogConfiguration GetLogConfiguration(ConfigurationType type, string product, string component)
        {
            //Check if a config for the given type exists, return null if not
            if (!_config.ContainsKey(type))
            {
                return null; 
            }
            if (_config[type] == null)
            {
                return null; 
            }
            //Create the Config and Return 
            return _config[type].ConfigurationCreator.Create(product, component, 
                                                             _config[type].GetHeaderString(), 
                                                             _config[type].EntryFormat); 
        }
        #endregion
    }
}
