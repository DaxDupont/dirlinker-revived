using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces.Views;

namespace DirLinker.Tests.Helpers
{
    public class StubMainView : ILinkerView
    {
        #region ILinkerView Members

        public string LinkPoint { get; set; }

        public string LinkTo { get; set; }

        public bool CopyBeforeDelete { get; set; }

        public bool OverWriteTargetFiles { get; set; }

        public string TempPath { get; set; }

        public System.Windows.Forms.Form MainForm { get; private set; }

        public void Setup()
        {
            throw new NotImplementedException();
        }

        //We know these are unused because it's a test double so disable the warnings
#pragma warning disable 0067
        public event PathValidater ValidatePath;
        public event PerformLink PerformOperation;
#pragma warning restore 0067
        #endregion

      
        public void SetOperationData(DirLinker.Data.LinkOperationData data)
        {
            throw new NotImplementedException();
        }


        #region ILinkerView Members


        public Func<bool> ValidOperation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ILinkerView Members


        public void ShowMesage(string message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
