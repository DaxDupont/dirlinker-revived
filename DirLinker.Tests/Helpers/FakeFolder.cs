using System;
using DirLinker.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace DirLinker.Tests.Helpers
{
    public class FakeFolder : IFolder
    {
        public FakeFolder()
        {
        }
        public FakeFolder(String path)
        {
            FolderPath = path;
        }

        #region IFolder Members

        public int MaxPathValue { get; set; }
        public int MaxPath()
        {
            return MaxPathValue;
        }

        [DefaultValue(false)]
        public bool CreateFolderCalled { get; set; }
        public void CreateFolder()
        {
            CreateFolderCalled = true;
        }

        [DefaultValue(false)]
        public bool DeleteFolderCalled { get; set; }
        public void DeleteFolder()
        {
            DeleteFolderCalled = true;
        }
        
        public List<IFile> FileList {get; set;}
        public List<IFile> GetFileList()
        {
            FileList = FileList ?? new List<IFile>();
            return FileList;
        }
        public List<IFolder> SubFolderList {get; set;}
        public List<IFolder> GetSubFolderList()
        {
            SubFolderList = SubFolderList ?? new List<IFolder>();
            return SubFolderList;
        }

        
        public String CreateLinkCallValue { get; set; }

        [DefaultValue(true)]
        public bool DeleteFolderReturnValue { get; set; }
        public bool CreateLinkToFolderAt(string linkToBeCreated)
        {
            CreateLinkCallValue = linkToBeCreated;
            return DeleteFolderReturnValue;
        }

        [DefaultValue(false)]
        public bool FolderExistsCalled { get; set; }
        [DefaultValue(true)]
        public bool FolderExistsReturnValue { get; set; }
        public bool FolderExists()
        {
            FolderExistsCalled = true;
            return FolderExistsReturnValue;
        }

        public FileAttributes FolderAttributes {get; set;}
        public void SetAttributes(FileAttributes fileAttributes)
        {
            FolderAttributes = fileAttributes;
        }

        public FileAttributes GetAttributes()
        {
            return FolderAttributes;
        }

        public long DirSize {get; set;}
        public long DirectorySize()
        {
            return DirSize;
        }

        public long FreeSpaceOnDriveReturnValue { get; set; }
        public long FreeSpaceOnDrive(string drive)
        {
            return FreeSpaceOnDriveReturnValue;
        }

        public char[] GetIllegalPathCharsReturnValue {get; set;}
        public char[] GetIllegalPathChars()
        {
            return GetIllegalPathCharsReturnValue;
        }

        public string FolderPath
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (obj is FakeFolder)
            {
                FakeFolder other = obj as FakeFolder;
                return this.FolderPath.Equals(other.FolderPath, StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
