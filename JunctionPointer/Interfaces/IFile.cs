using System;

namespace DirLinker.Interfaces
{
    public delegate IFile IFileFactoryForPath(String filePath);
    public interface IFile
    {

        bool CreateLinkToFileAt(String linkToBeCreated);
        void MoveFile(IFile target);

        String FileName { get; }
        String Folder { get; }
        String FullFilePath { get; }

        System.IO.FileAttributes GetAttributes();

        void CopyFile(IFile fullTargetPathWithFileName, Boolean overwrite);
        void SetAttributes(System.IO.FileAttributes attributes);
        void Delete();
        
        Boolean Exists();
    }
}
