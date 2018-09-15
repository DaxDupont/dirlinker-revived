using System;
using System.Collections.Generic;
using System.IO;

namespace DirLinker.Interfaces
{
    public enum SYMBOLIC_LINK_FLAG
    {
        File = 0,
        Directory = 1
    }

    public enum DirLinkerStage
    {
        None,
        CopyingSourceToTemp,
        CreatingDirectoryLink,
        CopyingTempToSource,
        Unknown
    }

    public delegate IFolder IFolderFactoryForPath(String folder);

    public interface IFolder
    {
        Int32 MaxPath();
        void CreateFolder();
        void DeleteFolder();
       

        List<IFile> GetFileList();
        List<IFolder> GetSubFolderList();
        
        Boolean CreateLinkToFolderAt(String linkToBeCreated);
        Boolean FolderExists();

        void SetAttributes(FileAttributes fileAttributes);
        FileAttributes GetAttributes();

        Int64 DirectorySize();
        Int64 FreeSpaceOnDrive(String drive);

        Char[] GetIllegalPathChars();

        String FolderPath { get; }
    }
}
