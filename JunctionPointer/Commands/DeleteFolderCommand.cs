using System;
using System.Linq;
using DirLinker.Interfaces;
using DirLinker.Exceptions;
using System.IO;

namespace DirLinker.Commands
{
    
    public class DeleteFolderCommand : ICommand
    {
        private IFolder _Folder;
        private Boolean _FolderDeleted;
        public DeleteFolderCommand(IFolder folder)
        {
            _Folder = folder;
        }

        public void Execute()
        {
            if (_Folder.FolderExists())
            {
                DeleteFolder(_Folder);
                _FolderDeleted = true;
            }
        }

        private void DeleteFolder(IFolder folder)
        { 
            folder.GetSubFolderList().ForEach(DeleteFolder);
            folder.GetFileList().ForEach(f => 
            {
                if ((f.GetAttributes() & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    f.SetAttributes(FileAttributes.Normal);
                }
                f.Delete();
            
            });

          
            folder.DeleteFolder();
        }

        public void Undo()
        {
            if (_FolderDeleted)
            {
                _Folder.CreateFolder();
            }
        }

        public string UserFeedback
        {
            get { return String.Format("Deleting folder {0}", _Folder.FolderPath); }
        }

#pragma warning disable 00067
        public event RequestUserReponse AskUser;
#pragma warning restore 00067

    }
}
