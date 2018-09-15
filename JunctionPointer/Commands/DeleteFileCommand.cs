using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;
using System.IO;

namespace DirLinker.Commands
{
    public class DeleteFileCommand : ICommand
    {
        private IFile _fileToDelete;
        public DeleteFileCommand(IFile fileToDelete)
        {
            _fileToDelete = fileToDelete;
        }

        public void Execute()
        {
            if (_fileToDelete.Exists())
            {
                CheckFilePermissions();
                _fileToDelete.Delete();
            }
        }

        private void CheckFilePermissions()
        {
            if ((_fileToDelete.GetAttributes() & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                _fileToDelete.SetAttributes(FileAttributes.Normal);
            }
        }

        public void Undo()
        {
           
        }

        public string UserFeedback
        {
            get { return String.Format("Deleting file {0}", _fileToDelete.FullFilePath); }
        }
#pragma warning disable 00067
        public event RequestUserReponse AskUser;
#pragma warning restore 00067

    }
}
