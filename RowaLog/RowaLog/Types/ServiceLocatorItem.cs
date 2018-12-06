using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Rowa.Lib.Log.Types
{
    /// <summary>
    /// Holds the Classes Managed by the ServiceLocator
    /// </summary>
    internal class ServiceLocatorItem : IDisposable
    {
        /// <summary>
        /// Id of the Item
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// Number of Instaces/References to the Item
        /// </summary>
        private long _instanceCount = 1;

        /// <summary>
        /// actual Object held by the Class
        /// </summary>
        private readonly object _memberObject;

        /// <summary>
        /// Id of the Item
        /// </summary>
        public Guid Id => _id;

        /// <summary>
        /// actual Object held by the Class
        /// </summary>
        public object MemberObject => _memberObject;

        /// <summary>
        /// Number of Instaces/References to the Item
        /// </summary>
        public long InstanceCount => _instanceCount;

        /// <summary>
        /// Default Class Constructor
        /// </summary>
        /// <param name="id">Id of the Item</param>
        /// <param name="t">type of the instanced Member</param>
        public ServiceLocatorItem(Guid id, Type t)
        {
            _id = id;
            _memberObject = Activator.CreateInstance(t);
        }

        /// <summary>
        /// Increases the Instance Counter
        /// </summary>
        /// <returns>Incemented Value</returns>
        public long IncInstance()
        {
            return Interlocked.Increment(ref _instanceCount);
        }

        /// <summary>
        /// Decreases the Instance Counter
        /// </summary>
        /// <returns>Decremented Value</returns>
        public long DecInstance()
        {
            return Interlocked.Decrement(ref _instanceCount);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_memberObject != null)
                    {
                        IDisposable disposablememberObject = _memberObject as IDisposable;

                        disposablememberObject?.Dispose();
                    }
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
