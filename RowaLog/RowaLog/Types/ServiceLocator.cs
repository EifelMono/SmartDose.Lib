using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Rowa.Lib.Log.Types
{
    internal static class ServiceLocator
    {
        /// <summary>
        /// holds the Items Managed by the Servicelocator
        /// </summary>
        private static readonly List<ServiceLocatorItem> _items = new List<ServiceLocatorItem>();

        /// <summary>
        /// Synchronization Object for Thread Safety
        /// </summary>
        private static readonly object _mutex = new object();

        /// <summary>
        /// Finds and Returns a Servicelocator Item
        /// </summary>
        /// <param name="id">Id of the Item</param>
        /// <param name="t">Type of the Item</param>
        /// <returns>a ServiceLocatorItem or null if nothing was found</returns>
        private static ServiceLocatorItem GetItem(Guid id, Type t)
        {
            return _items.Find(x => x.Id == id && x.MemberObject.GetType() == t);
        }

        /// <summary>
        /// Finds and Returns a Servicelocator Item
        /// </summary>
        /// <param name="item">item to be found</param>
        /// <returns>a ServiceLocatorItem or null if nothing was found</returns>
        private static ServiceLocatorItem GetItem(object item)
        {
            return _items.Find(x => x.MemberObject == item);
        }

        /// <summary>
        /// Disposes the given Item if possible, 
        /// item will be set to its default value anyways
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object that should be disposed</param>
        private static void DisposeElement<T>(ref T item)
        {
            var disposableItem = item as IDisposable;
            disposableItem?.Dispose();
            item = default(T);
        }

        /// <summary>
        /// Returns the same Instace of a Type
        /// </summary>
        /// <typeparam name="T">Type you need</typeparam>
        /// <returns>Instance of a type</returns>
        public static T Get<T>()
        {
            return Get<T>(Guid.Empty);
        }

        /// <summary>
        /// Returns the same Instace of a Type with the given Id
        /// </summary>
        /// <typeparam name="T">Type you need</typeparam>
        /// <param name="id">Id you wish to store the tyoe into</param>
        /// <returns>Instance of a type</returns>
        public static T Get<T>(Guid id)
        {
            lock(_mutex)
            {
                var diResult = GetItem(id, typeof(T));

                if (diResult == null)
                {
                    diResult = new ServiceLocatorItem(id, typeof(T));
                    _items.Add(diResult);

                } else
                {
                    diResult.IncInstance();
                }


                return (T)diResult.MemberObject;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public static void DisposeObject<T>(ref T item)
        {
            lock (_mutex)
            {
                var member = GetItem(item);
                if (member != null)
                {
                    //Member is part of the Service locator 
                    member.DecInstance();

                    if (member.InstanceCount == 0)
                    {
                        _items.Remove(member);
                        DisposeElement(ref member); 
                    }
                } else
                {
                    //The Member that should be disposed does not 
                    //exist in the Service locator -> Dispose not service
                    //locates managed object 
                    DisposeElement(ref item); 
                }
            }
        }

        public static void ForceDisposeAll()
        {
            lock (_mutex)
            {
                
                foreach(var item in _items)
                {
                    try
                    {
                        item.Dispose();
                    }
                    catch { }
                }

                _items.Clear();
            }
        }
    }
}
