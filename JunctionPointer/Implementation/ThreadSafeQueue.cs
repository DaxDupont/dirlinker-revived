using System;
using System.Collections.Generic;
using DirLinker.Interfaces;

namespace DirLinker.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSafeQueue<T>
    {

        private ILocker _locker;
        private Queue<T> _interalQueue =  new Queue<T>();

        public ThreadSafeQueue(ILocker locker) 
        {
            _locker = locker;
        }

        public Int32 Count
        {
            get
            {
                return _interalQueue.Count;
            }
        }

        public void Enqueue(T obj)
        {
            using (_locker.AcquireLock())
            {
                _interalQueue.Enqueue(obj);
            }
        }

        public T Dequeue()
        {
            using (_locker.AcquireLock())
            {
               return _interalQueue.Dequeue();
            }
        }

        public IEnumerable<T> ProcessQueue()
        {
            while (_interalQueue.Count > 0)
            {
                yield return Dequeue();
            }
        }

        public void Clear()
        {
            _interalQueue.Clear();
        }
    }
}
