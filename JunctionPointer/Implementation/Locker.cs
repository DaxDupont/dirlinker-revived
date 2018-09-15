using System;
using System.Threading;
using DirLinker.Interfaces;

namespace DirLinker.Implementation
{
    /// <summary>
    /// This class acquires lock through Montior.Enter and allows the consumer to wrap the lock
    /// in a using statement similar to the lock keyword.  
    /// 
    /// The Locker has one internal object that is used to lock against.  This will provide large area locks
    /// IE if you lock in Function A and try to lock Function B with the same Locker instance Function B will 
    /// block until Function A has released the lock.  This is deliberate for use in ThreadSafeQueue but it can 
    /// lead to deadlocks so be careful and deligent with its use.
    /// 
    /// This has been introduced to vary the lock as needed for example between a monitor 
    /// and a reader writer and aids 'testability'
    /// </summary>

    public class Locker : ILocker
    {
        private Object _PadLock = new Object();

        /// <summary>
        /// This is our internal representation of the lock we acquired.
        /// </summary>
        private class AcquiredLock : IDisposable
        {
            private Locker _LockedObject;

            public AcquiredLock(Locker lockedObject)
            {
                _LockedObject = lockedObject;
            }
            /// <summary>
            /// releases the lock
            /// </summary>
            public void Dispose()
            {
                _LockedObject.ReleaseLock();
            }
        }
       /// <summary>
       /// Acquired the lock
       /// </summary>
       /// <returns>An IDispoable that can be used in a using statement to automatically release the lock</returns>
        public IDisposable AcquireLock()
        {
            Monitor.Enter(_PadLock);

            return new AcquiredLock(this);
        }

        /// <summary>
        /// Releases the lock
        /// </summary>
        protected void ReleaseLock()
        {
            Monitor.Exit(_PadLock);
        }
    }
}
