using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;
using DirLinker.Exceptions;

namespace DirLinker.Commands
{
   
    public class CreateFolderLinkCommand : ICommand
    {
        private IFolder _linkTo;
        private IFolder _linkFrom;

        public CreateFolderLinkCommand(IFolder linkTo, IFolder linkFrom)
        {
            _linkTo = linkTo;
            _linkFrom = linkFrom;
        }

        
        public void Execute()
        {
            if (!_linkTo.FolderExists())
            {
                _linkFrom.CreateLinkToFolderAt(_linkTo.FolderPath);
            }
            else
            {
                throw new DirLinkerException("A link cannot be created if the a folder exists at the target", DirLinkerStage.CreatingDirectoryLink);
            }
        }

        public void Undo()
        {
            throw new NotSupportedException("This operation is not supported for this command.");
        }

        public string UserFeedback
        {
            get { return String.Format("Creating symbolic link at {0}", _linkTo.FolderPath); }
        }
#pragma warning disable 00067
        public event RequestUserReponse AskUser;
#pragma warning restore 00067

    }
}
