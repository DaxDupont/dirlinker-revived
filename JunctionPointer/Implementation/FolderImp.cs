using System;
using System.Collections.Generic;
using System.Linq;
using DirLinker.Interfaces;
using System.IO;
using System.Runtime.InteropServices;
using OCInject;

namespace DirLinker.Implementation
{
    class FolderImp : IFolder
    {
        public String FolderPath { get; protected set; }

        IFileFactoryForPath FileFactoryForFile;
        IFolderFactoryForPath FolderFactoryForPath;

        [DllImport("kernel32.dll", SetLastError=true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I1)]
        
        private static extern bool CreateSymbolicLink(String lpSymlinkFileName, String lpTargetFileName, SYMBOLIC_LINK_FLAG dwFlags);

        public bool CreateLinkToFolderAt(String linkToBeCreated)
        {
            return CreateSymbolicLink(linkToBeCreated, FolderPath, SYMBOLIC_LINK_FLAG.Directory);
        }

        public FolderImp(IFileFactoryForPath fileFactory, IFolderFactoryForPath folderFactory, String path)
        {
            FolderPath = path;
            FileFactoryForFile = fileFactory;
            FolderFactoryForPath = folderFactory;
        }
        

        public Boolean FolderExists()
        {
            return Directory.Exists(FolderPath);
        }

        public void CreateFolder()
        {
            Directory.CreateDirectory(FolderPath);
        }

        /// <summary>
        /// Deletes a folder and its children if it exists
        /// </summary>
        /// <param name="foldername"></param>
        public void DeleteFolder()
        {
            // There has been a bug here where we are trying to delete the current path, strange but sadly true
            //So we need to check it and change it
            
            var dirInfo = new DirectoryInfo(FolderPath);
            try
            {
                if (Directory.GetCurrentDirectory().Equals(FolderPath, StringComparison.CurrentCultureIgnoreCase))
                {
                    Directory.SetCurrentDirectory(dirInfo.Parent.FullName);
                }
                dirInfo.Delete();

            }
            catch (IOException)
            {
                //Block here to allow the OS to catch up on all the deletes.
                //The user can have the path open in explorer or another process so we need wait for it to be closed
                //This is here because of an intermitent bug.

                //My best guess is that a subfolder is being held open by explorer or the folder browser preventing it from being closed
                //so that it's not deleted and the folder can not be deleted because it's not empty.   I can't reproduce it now to comfirm this via procmon
                System.Threading.Thread.Sleep(0);
                dirInfo.Delete();
            }
        }

        /// <summary>
        /// Gets a list of files in a folder
        /// </summary>
        /// <param name="folderLocation"></param>
        /// <returns></returns>
        public List<IFile> GetFileList()
        {
            List<IFile>  fileList = new List<IFile>();

            foreach (String file in Directory.GetFiles(FolderPath))
            {
                IFile aFile = FileFactoryForFile(file);
                fileList.Add(aFile);
            }

            return fileList;
             
        }

        /// <summary>
        /// Gets the size of a folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns>The size of a folder</returns>
        public Int64 DirectorySize()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(FolderPath);

            Int64 dirSize = dirInfo.GetFiles().Sum(file => file.Length);
            dirSize += GetSubFolderList().Sum(dir => dir.DirectorySize());

            return dirSize;
        }

        public Int64 FreeSpaceOnDrive(String drive)
        {
            DriveInfo driveInfo = new DriveInfo(drive);
            return driveInfo.AvailableFreeSpace;

        }

        public List<IFolder> GetSubFolderList()
        {
            List<IFolder> folderList = new List<IFolder>();

            foreach (string subFolder in Directory.GetDirectories(FolderPath))
            {
                IFolder folder = FolderFactoryForPath(subFolder);
                folderList.Add(folder);
            }

            return folderList;
        }

        public Char[] GetIllegalPathChars()
        {
            return Path.GetInvalidPathChars();
        }

        public Int32 MaxPath()
        {
            return 248; //According to MSDN 248 is the max folder path for Windows, strange there is not a Path member to get this.
        }

        public void SetAttributes(FileAttributes fileAttributes)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(FolderPath);
            dirInfo.Attributes = fileAttributes;
        }
        public FileAttributes GetAttributes()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(FolderPath);
            return dirInfo.Attributes;
        }
        

    }
}
