using System;
using System.Collections.Generic;
using System.Threading;

namespace Rowa.Lib.Wcf.Gui
{
    /// <summary>
    /// Class which implements the logic of a thread safe message queue with notification mechanism.
    /// </summary>
    internal class MessageQueue : IDisposable
    {
        #region Members

        /// <summary>
        /// Queue which holds the messages which are pending for processing.
        /// </summary>
        private Queue<string> _messageQueue = new Queue<string>();

        /// <summary>
        /// Event which is notified when the message queue has changed.
        /// </summary>
        private AutoResetEvent _queueChangedEvent = new AutoResetEvent(false);

        /// <summary>
        /// Event that is notified when the queue has been cancelled.
        /// </summary>
        private ManualResetEvent _cancelEvent = new ManualResetEvent(false);

        /// <summary>
        /// Flag wether this object already has been disposed.
        /// </summary>
        private bool _isDisposed = false;

        #endregion

        /// <summary>
        /// Enqueues the specified message and notifies the according wait method.
        /// </summary>
        /// <param name="message">
        /// The message to enqueue.
        /// </param>
        public void Enqueue(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Invalid message specified.");
            }

            lock (_messageQueue)
            {
                _messageQueue.Enqueue(message);
            }

            _queueChangedEvent.Set();
        }

        /// <summary>
        /// Waits for a message which can be dequeued for the specified amount of time.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The timeout in milliseconds to wait for a  message to dequeue.
        /// 0 means to wait infinitely.
        /// </param>
        /// <returns>
        /// The dequeued message if successful; null otherwise.
        /// </returns>
        public string WaitForDequeue(int millisecondsTimeout = 0)
        {
            lock (_messageQueue)
            {
                if (_messageQueue.Count > 0)
                {
                    return _messageQueue.Dequeue();
                }
            }

            var waitHandles = new WaitHandle[] { _queueChangedEvent, _cancelEvent };
            var waitResult = (millisecondsTimeout > 0) ? WaitHandle.WaitAny(waitHandles, millisecondsTimeout) : 
                                                         WaitHandle.WaitAny(waitHandles);

            if ((waitResult == WaitHandle.WaitTimeout) ||
                (waitHandles[waitResult] == _cancelEvent))
            {
                return null;
            }

            lock (_messageQueue)
            {
                if (_messageQueue.Count > 0)
                {
                    return _messageQueue.Dequeue();
                }
            }

            return null;
        }

        /// <summary>
        /// Cancels any pending wait operations (e.g. waiting for dequeue).
        /// </summary>
        public void Cancel()
        {
            _cancelEvent.Set();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                _cancelEvent.Dispose();
                _queueChangedEvent.Dispose();
                _isDisposed = true;
            }
        }

    }
}
