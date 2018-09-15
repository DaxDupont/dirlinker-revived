using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;
using System.ComponentModel;

namespace DirLinker.Tests.Helpers
{
    class FakeBackgroundWorker : IBackgroundWorker
    {
        public event System.ComponentModel.DoWorkEventHandler DoWork;
        //We know these are unused because it's a test double so disable the warnings
#pragma warning disable 0067
        public event System.ComponentModel.RunWorkerCompletedEventHandler RunWorkerCompleted;
#pragma warning restore 0067
        public void RunWorkerAsync()
        {
            RunWorkerAsync(null);
        }

        public void RunWorkerAsync(Object args)
        {
            DoWork(this, new DoWorkEventArgs(args));
        }

    }
}
