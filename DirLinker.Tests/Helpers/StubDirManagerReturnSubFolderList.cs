using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;
using System.IO;

namespace DirLinker.Tests.Helpers
{
    class StubDirManagerReturnSubFolderList : IFolder
    {
        public StubDirManagerReturnSubFolderList()
        {
            FolderList = new List<IFolder>();
            
        }

        public IFolder OnlyReturnSubFolderListFor {get; set; }

        public List<IFolder> FolderList{get; set;}

        public int MaxPath()
        {
            return 254;
        }

        public void CreateFolder()
        {
        }

        public void DeleteFolder()
        {
        }

        public List<IFile> GetFileList()
        {
            return new List<IFile>();
        }

        public List<IFolder> GetSubFolderList()
        {
            return FolderList;
        }

        public bool CreateLinkToFolderAt(String linkToBeCreated)
        {
            return true;
        }

        public bool FolderExists()
        {
            return true;
        }

        public long DirectorySize()
        {
            return 0;
        }

        public long FreeSpaceOnDrive(string drive)
        {
            return 1;
        }

        public char[] GetIllegalPathChars()
        {
            return new char[0];
        }


        public void SetAttributes(FileAttributes fileAttributes)
        { 
        }

        public FileAttributes GetAttributes()
        {
            return FileAttributes.Normal;
        }
    

        public string FolderPath
        {
            get;
            set;
        }

    }
}
