using System;
using DirLinker.Interfaces;

namespace DirLinker.Commands
{
    public delegate ICommand CreateFolderCommandFactory(IFolder folder);

    public class CreateFolderCommand : ICommand
    {
        private IFolder _Folder;
        private Boolean _FolderCreatedByCommand;

        public CreateFolderCommand(IFolder folder)
        {
            _Folder = folder;
            _FolderCreatedByCommand = false;
        }

        public void Execute()
        {
            if (!_Folder.FolderExists())
            {
                _Folder.CreateFolder();
                _FolderCreatedByCommand = true;
            }
        }

        public void Undo()
        {
            if (_FolderCreatedByCommand)
            {
                _Folder.DeleteFolder();
            }
        }

        public string UserFeedback
        {
            get { return String.Format("Creating folder {0}", _Folder.FolderPath); }
        }

#pragma warning disable 00067
        public event RequestUserReponse AskUser;
#pragma warning restore 00067
    }
}
