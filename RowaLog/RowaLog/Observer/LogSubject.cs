using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Rowa.Lib.Log.Observer
{
    internal class LogSubject : IDisposable
    {
        #region ------------- Fields -------------
        /// <summary>
        /// List of Observer this Subject should notify
        /// </summary>
        private List<IObserver> _observer;

        /// <summary>
        /// Lock that is used to Lock the _observer List 
        /// Because the LogSubject works asynchron 
        /// </summary>
        private ReaderWriterLockSlim _observerLock;
        #endregion

        #region ------------- Konstruktor -------------
        /// <summary>
        /// Creates a new Instance of the LogSubject class that can be used
        /// to balance messages between the Observer that subscribe to the 
        /// observer
        /// </summary>
        internal LogSubject()
        {
            Initialize();
        }
        #endregion

        #region ------------- Methods private -------------
        /// <summary>
        /// Intializes every Field that is needed in the class
        /// </summary>
        private void Initialize()
        {
            _observer = new List<IObserver>();
            _observerLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Returns a List that is a Copy of the internal Observer List 
        /// </summary>
        /// <returns></returns>
        private List<IObserver> GetObserverCopy()
        {
            List<IObserver> _copy;
            _observerLock.EnterReadLock();
            try
            {
                _copy = new List<IObserver>(_observer);
            }
            finally
            {
                _observerLock.ExitReadLock();
            }
            return _copy;
        }

        #endregion


        #region ------------- Methods public -------------

        /// <summary>
        /// Adds the given Observer to the List of observer that get Notified in case of 
        /// a Notification 
        /// </summary>
        /// <param name="observer"></param>
        internal void SubscribeObserver(IObserver observer)
        {
            _observerLock.EnterWriteLock();
            try
            {
                _observer.Add(observer);
            }
            finally
            {
                _observerLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Unsubscribes the given Observer from the internal subsribtion list. 
        /// </summary>
        /// <param name="observer"></param>
        internal void UnsubscribeObserver(IObserver observer)
        {
            _observerLock.EnterWriteLock();
            try
            {
                if (_observer.Contains(observer))
                {
                    _observer.Remove(observer);
                }
            }
            finally
            {
                _observerLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Notifies all subscribed Observer with the given Notification 
        /// </summary>
        /// <param name="notification"></param>
        internal void Notify(Notification notification)
        {
            List<IObserver> _observerCopy = GetObserverCopy();

            foreach (IObserver observer in _observerCopy)
            {
                observer.onNotify(notification);
            }
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
                    _observerLock.Dispose();
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

