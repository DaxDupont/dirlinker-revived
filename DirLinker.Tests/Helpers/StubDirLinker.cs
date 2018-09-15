using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;

namespace DirLinker.Tests.Helpers
{
    public class StubDirLinker : IDirLinker
    {

        public String LinkTo;
        public String LinkPoint;
        public String Temp;
        public Boolean CopyBeforeDelete;
        public Boolean OverwriteTargetFiles;
        //We know these are unused because it's a test double so disable the warnings
#pragma warning disable 0067
      //  public event UserMessage UserMessage;

        public event ReportProgress ReportFeedback;
#pragma warning restore 0067

        public bool CheckEnoughSpace(string target, string tempPath)
        {
            throw new NotImplementedException();
        }

        public void CreateSymbolicLinkFolder(String linkPoint, String linkTo, bool copyBeforeDelete, bool overwriteTargetFiles)
        {
            LinkPoint = linkPoint;
            LinkTo = linkTo;
            OverwriteTargetFiles = overwriteTargetFiles;
            CopyBeforeDelete = copyBeforeDelete;
        }

        public void CopyFolder(IFolder source, IFolder target, bool overwriteTargetFiles)
        {
            throw new NotImplementedException();
        }



        public bool ValidDirectoryPath(string path, out string ErrorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
