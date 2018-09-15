using System;
using DirLinker.Interfaces;
using System.IO;
using System.Runtime.InteropServices;

namespace DirLinker.Implementation
{
    class FileImp : IFile
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I1)]
        private static extern bool CreateSymbolicLink(String lpSymlinkFileName, String lpTargetFileName, SYMBOLIC_LINK_FLAG dwFlags);

        public FileImp(String fullPath)
        {
            m_FullPath = fullPath;
        }
             
        private String m_FullPath;

        public string FileName
        {
            get
            {
                return Path.GetFileName(m_FullPath);
            }
        }

        public string Folder
        {
            get
            {
                return Path.GetDirectoryName(m_FullPath);
            }
        }

        public String FullFilePath 
        {
            get
            {
                return m_FullPath;
            }
        }

        public void CopyFile(IFile fullTargetPathWithFileName, Boolean overwrite)
        {
            File.Copy(m_FullPath, fullTargetPathWithFileName.FullFilePath, overwrite);
        }

        public FileAttributes GetAttributes()
        {
            return File.GetAttributes(m_FullPath);
        }

        public void SetAttributes(FileAttributes attributes)
        {
            File.SetAttributes(m_FullPath, attributes);
        }

        public Boolean Exists()
        {
            return File.Exists(m_FullPath);
        }

        public void Delete()
        {
            File.Delete(FullFilePath);
        }

        public void MoveFile(IFile target)
        {
            File.Move(m_FullPath, target.FullFilePath);
        }

        public bool CreateLinkToFileAt(String linkToBeCreated)
        {
            return CreateSymbolicLink(linkToBeCreated, m_FullPath, SYMBOLIC_LINK_FLAG.File);
        }
    }
}
