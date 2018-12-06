using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rowa.Lib.Log.Types;
using Rowa.Lib.Log.Observer;

namespace Rowa.Lib.Log.Appender
{
    /// <summary>
    /// This Appender does Asynchronous Console Logging
    /// </summary>
    internal class LogAppenderConsoleAsync : LogAppenderConsole, IDisposable
    {
        #region Members
        /// <summary>
        /// Loop for Logging
        /// </summary>
        private LogAsyncLoop _logLoop;
        #endregion

        #region non-public Methods
        /// <summary>
        /// Adds a LogEntry to the LogQueue
        /// </summary>
        /// <param name="entry">Entry to be added</param>
        /// <returns>true if Adding Successful</returns>
        private bool DoEnqueue(LogEntryExt entry)
        {
            var action = new LogAction
            {
                Action = WriteConsole,
                Entry = entry
            };

            return _logLoop.TryEnqueue(action);
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Initializes the Appender
        /// </summary>
        /// <param name="config">Config used for initialization</param>
        public override void Initialize(AppenderConfiguration config, LogSubject logSubject)
        {
            base.Initialize(config, logSubject);

            _logLoop = ServiceLocator.Get<LogAsyncLoop>(new Guid("293317da-291e-48bd-80c1-f9e61372c6da"));
            _logLoop.Initialize();
        }

        /// <summary>
        /// Appends a LogEntry to the Logging Queue for Async Logging
        /// </summary>
        /// <param name="entry">LogEntry to be appended</param>
        public override void Append(LogEntryExt entry)
        {
            if (entry == null) return;

            if (DoEnqueue(entry)) return;

            long amountCleared = _logLoop.Clear();
            DoEnqueue(new LogEntryExt
            (
                new LogEntry
                {
                    Level = LogLevel.Fatal,
                    Format = $"{amountCleared} Log entries Deleted from Log Queue."
                }, 
                _subject
            ));

            ExecuteLoggerError(new LogErrorEventArgs { Error = LoggerError.LogQueueFull, Message = "Log Queue is Full!" });
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _logLoop.WaitLoopEmpty(100);
                    ServiceLocator.DisposeObject(ref _logLoop);
                }

                disposedValue = true;
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
