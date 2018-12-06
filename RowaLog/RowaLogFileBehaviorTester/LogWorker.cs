using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rowa.Lib.Log;

namespace RowaLogFileBehaviorTester
{
    class LogWorker
    {
        private readonly string _logEntry;
        private readonly int _breakBetweenEntries;
        private bool _started;
        private int _logCount;
        private readonly ILog _logger;
        private CancellationTokenSource _cancellation;
        private readonly ManualResetEvent _waitEvent;

        public int LogCount => _logCount;

        public LogWorker(string logEntry, int breakBetweenEntries, ILog logger)
        {
            _waitEvent = new ManualResetEvent(false);
            _logEntry = logEntry;
            _breakBetweenEntries = breakBetweenEntries;
            _logger = logger;
        }

        public void Start()
        {
            if (_started) return;

            _logCount = 0;
            _cancellation = new CancellationTokenSource();
            _waitEvent.Reset();
            _started = true;

            Task.Factory.StartNew(() =>
            {
                while (!_cancellation.IsCancellationRequested)
                {
                    _logger.Info(_logEntry);
                    _logger.Audit(_logEntry);
                    _logger.Debug(_logEntry);
                    _logger.ExtIf(_logEntry);
                    _logger.Error(_logEntry);
                    _logger.Fatal(_logEntry);
                    _logger.UserIn(_logEntry);
                    _logger.Warning(_logEntry);
                    Interlocked.Increment(ref _logCount);

                    if (_breakBetweenEntries > 0)
                    {
                        _waitEvent.WaitOne(_breakBetweenEntries);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            if (!_started) return;

            _waitEvent.Set();
            _cancellation.Cancel();
            _started = false;
        }
    }
}


