using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;
using System.IO;

namespace DirLinker.Tests.Helpers
{
    public class FakeFile : IFile
    {
        public String FileLinkCreatedAt { get; set; }
        
        public bool CreateLinkToFileAt(String linkToBeCreated)
        {
            FileLinkCreatedAt = linkToBeCreated;
            return true;
        }

        public void MoveFile(IFile _Target)
        {
            throw new NotImplementedException();
        }

        public FakeFile(String filename)
        {
            FullFilePath = filename;
        }



    
        public string FileName
        {
            get;
            set;
        }

        public string Folder
        {
            get;
            set;
        }

        public string FullFilePath
        {
            get;
            set;
        }

        public FileAttributes Attributes { get; set; }
        public System.IO.FileAttributes GetAttributes()
        {
            return Attributes;
        }

        [System.ComponentModel.DefaultValue(false)]
        public bool CopyFileCalled { get; set; }
        public void CopyFile(IFile fullTargetPathWithFileName, bool overwrite)
        {
            CopyFileCalled = true;
        }

        public void SetAttributes(System.IO.FileAttributes attributes)
        {
            Attributes = attributes;
        }

        [System.ComponentModel.DefaultValue(false)]
        public bool SetFileCalled { get; set; }
        public void SetFile(string fileWithFullPath)
        {
            SetFileCalled = true;
        }

        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteCalled { get; set; }
        public void Delete()
        {
            DeleteCalled = true;
        }

        [System.ComponentModel.DefaultValue(false)]
        public bool ExistsReturnValue { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool ExistsCalled { get; set; }
        public bool Exists()
        {
            ExistsCalled = true;
            return ExistsReturnValue;
        }

    }
}
