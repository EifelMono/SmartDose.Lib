using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rowa.Lib.Log.Appender;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Observer;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// This Factory Creates Appenders corresponding to the config provided
    /// </summary>
    internal class AppenderFactory
    {
        /// <summary>
        /// Makes sure that Instance Creation is ThreadSafe
        /// </summary>
        private readonly object _mutex = new object();

        /// <summary>
        /// Returns an Appender from an AppenderConfiguration
        /// </summary>
        /// <param name="config">A Configuration to Create an Appender from</param>
        /// <returns>a Log Appender</returns>
        public ILogAppender GetAppender(AppenderConfiguration config, LogSubject logSubject)
        {
            if (config == null) return null;
            if (config.AppenderType == null) return null;
            if (config.AppenderType.GetInterface(typeof(ILogAppender).ToString()) == null) return null;

            try
            {
                lock(_mutex)
                {
                    ILogAppender instance = (ILogAppender)Activator.CreateInstance(config.AppenderType);
                    try
                    {
                        instance.Initialize(config, logSubject);

                        return instance;
                    }
                    catch
                    {
                        var disposableInstance = instance as IDisposable;

                        disposableInstance?.Dispose();
                    }
                }
            }
            catch
            {
                //When Anything goes wrong we just want to return null
            }

            return null;
        }

    }
}
