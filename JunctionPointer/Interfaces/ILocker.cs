using System;

namespace DirLinker.Interfaces
{
    public interface ILocker
    {
        IDisposable AcquireLock();
    }
}
