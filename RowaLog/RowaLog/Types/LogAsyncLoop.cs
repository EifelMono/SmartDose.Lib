using Rowa.Lib.Log.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text; 
using System.Threading;

namespace Rowa.Lib.Log.Types
{
    internal class LogAsyncLoop : IDisposable
    {
        #region Members
        /// <summary>
        /// Mutex for ThreadSafe initialization
        /// </summary>
        private readonly object _mutex = new object();

        /// <summary>
        /// Queue for Log Entries
        /// </summary>
        private readonly LogQueue<LogAction> _logEntries = new LogQueue<LogAction>(1000000);

        /// <summary>
        /// Event to Signal when a new Log Entry is available
        /// </summary>
        private readonly AutoResetEvent _newLogEvent = new AutoResetEvent(false);

        /// <summary>
        /// Event for Checking if theres are any Actions Pending
        /// </summary>
        private readonly ManualResetEventSlim _logActionPending = new ManualResetEventSlim();

        /// <summary>
        /// Token to Cancel the Logging Loop
        /// </summary>
        private readonly CancellationTokenSource _cancelStreamOperationToken = new CancellationTokenSource();

        /// <summary>
        /// This is the asynchronous Loop inside the Class
        /// </summary>
        private Thread _logLoop;
        #endregion

        /// <summary>
        /// Initializes the Class
        /// </summary>
        public void Initialize()
        {
            lock(_mutex)
            {
                if (_logLoop != null) return;

                _logLoop = new Thread(ExecuteLoop);

                _logLoop.Name = "RowaLog Async LogLoop Thread";
                _logLoop.Start();
            }
        }

        /// <summary>
        /// Represents the LogLoop Main Method
        /// </summary>
        private void ExecuteLoop()
        {
            try
            {
                do
                {
                    if (_logEntries.IsEmpty)
                    {
                        _newLogEvent.WaitOne(100);
                        continue;
                    }

                    LogAction action;

                    while (!_cancelStreamOperationToken.IsCancellationRequested && _logEntries.TryDequeue(out action))
                    {
                        _logActionPending.Set();
                        //While the action could not be executed and no Cancleation is requests, Sleep and try again!
                        var logActionResult = false;

                        while (!logActionResult && !_cancelStreamOperationToken.IsCancellationRequested)
                        {
                            try
                            {
                                logActionResult = action.Action(action.Entry);

                            }
                            catch
                            {
                                logActionResult = false;
                            }

                            if (!logActionResult && !_cancelStreamOperationToken.IsCancellationRequested)
                            {
                                Thread.Sleep(50);
                            }
                        }
                        _logActionPending.Reset();
                    }

                } while (!_cancelStreamOperationToken.IsCancellationRequested);
            }
            catch (ObjectDisposedException)
            {
                //When this class is Disposed we don't need the Loop Anymore and All Disposed Errors can be ignored
            }
        }

        /// <summary>
        /// Adds a LogEntry to the LogQueue
        /// </summary>
        /// <param name="entry">Entry to be added</param>
        /// <returns>true if Adding Successful</returns>
        public bool TryEnqueue(LogAction action)
        {
            if (_logEntries.TryEnqueue(action))
            {
                _newLogEvent.Set();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clears the Loop
        /// </summary>
        /// <returns>Number of Entries that have been deleted</returns>
        public long Clear()
        {
            return _logEntries.Clear();
        }


        /// <summary>
        /// Waits for a Specific time if the Log Loop is Empty
        /// </summary>
        /// <param name="timeoutMs">Max Wait Time in MS</param>
        /// <returns></returns>
        public bool WaitLoopEmpty(int timeoutMs)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            using (var waitEvent = new ManualResetEventSlim())
            {

                do
                {
                    if (_logEntries.IsEmpty && !_logActionPending.IsSet) return true;

                    if (watch.ElapsedMilliseconds >= timeoutMs)
                    {
                        waitEvent.Set();
                    }

                } while (!waitEvent.Wait(10));
            }

            return false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cancelStreamOperationToken.Cancel();
                    _logLoop?.Join(100);
                    _logLoop = null;


                    _logEntries.Clear();

                    _newLogEvent.Dispose();
                    _logActionPending.Dispose();
                    _cancelStreamOperationToken.Dispose();
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
