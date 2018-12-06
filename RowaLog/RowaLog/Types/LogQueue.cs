using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Thread Safe Queue with a Max Limit for adding.
    /// </summary>
    internal class LogQueue<T> : IEnumerable<T>
    {
        #region Members
        /// <summary>
        /// Max Queue Size
        /// </summary>
        private long _maxSize;

        /// <summary>
        /// Current Size of the Queue
        /// </summary>
        private long _count;

        /// <summary>
        /// internal Queue to Store Data
        /// </summary>
        private readonly ConcurrentQueue<T> _queue;

        /// <summary>
        /// Current Size of the Queue
        /// </summary>
        public long Count => Interlocked.Read(ref _count);

        /// <summary>
        /// Max Queue Size
        /// </summary>
        public long MaxSize => Interlocked.Read(ref _maxSize);

        /// <summary>
        /// Returns if the Queue is Empty
        /// </summary>
        public bool IsEmpty => Interlocked.Read(ref _count) == 0;
        #endregion

        /// <summary>
        /// Constructor...
        /// </summary>
        /// <param name="maxSize">Maximum Size of the Queue</param>
        public LogQueue(long maxSize)
        {
            _maxSize = maxSize;
            _count = 0;
            _queue = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// Tries to add an item to the queue
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <returns>true if successful</returns>
        public bool TryEnqueue(T item)
        {
            if (Interlocked.Increment(ref _count) <= _maxSize)
            {
                _queue.Enqueue(item);

                return true;
            }


            Interlocked.Decrement(ref _count);
            return false;          
        }

        /// <summary>
        /// Tries to remove an Item from the Queue
        /// </summary>
        /// <param name="item">item from the Queue</param>
        /// <returns>true if successful</returns>
        public bool TryDequeue(out T item)
        {
            bool result = _queue.TryDequeue(out item);

            if (result) Interlocked.Decrement(ref _count);
            
            return result;
        }

        /// <summary>
        /// Tries to get an Item from the Queue without removing it.
        /// </summary>
        /// <param name="item">item fro the Queue</param>
        /// <returns>true if succesful</returns>
        public bool TryPeek(out T item)
        {
            return _queue.TryPeek(out item);
        }

        /// <summary>
        /// Removes all items from the Queue
        /// </summary>
        /// <returns></returns>
        public long Clear()
        {
            T outvar;
            long result = 0;

            while (TryDequeue(out outvar))
            {
                Interlocked.Increment(ref result);
            }
            
            return result;
        }

        /// <summary>
        /// Copies the Queue items to an array
        /// </summary>
        /// <param name="array">destination array</param>
        /// <param name="count">number of items to be copied</param>
        public void CopyTo(T[] array, int count)
        {
            _queue.CopyTo(array, count);
        }

        /// <summary>
        /// Copies Queue Elements to a anew Array.
        /// </summary>
        /// <returns>Array with Queue items</returns>
        public T[] ToArray()
        {
            return _queue.ToArray();
        }

        /// <summary>
        /// returns an enumerator to iterate through the Queue
        /// </summary>
        /// <returns>enumerator to iterate through queue</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        /// <summary>
        /// Supports simple iteration through queue
        /// </summary>
        /// <returns>enumerator to iterate through queue</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
