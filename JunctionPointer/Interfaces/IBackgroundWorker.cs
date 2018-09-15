using System;
using System.ComponentModel;

namespace DirLinker.Interfaces
{
    public interface IBackgroundWorker
    {
        event DoWorkEventHandler DoWork;
        event RunWorkerCompletedEventHandler RunWorkerCompleted;
        void RunWorkerAsync();
        void RunWorkerAsync(Object param);
    }
}
