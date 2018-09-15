using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;

namespace DirLinker.Commands
{
    public class CreateFileLinkCommand : ICommand
    {
        private IFile _linkFrom;
        private IFile _linkTo;

        public CreateFileLinkCommand(IFile linkTo, IFile linkfrom)
        {
            _linkTo = linkTo;
            _linkFrom = linkfrom;
        }

        public void Execute()
        {
            _linkFrom.CreateLinkToFileAt(_linkTo.FullFilePath);
        }

        public void Undo()
        {
        }

        public string UserFeedback
        {
            get { return "Creating file link"; }
        }
#pragma warning disable 0067
        public event RequestUserReponse AskUser;
#pragma warning restore 0067
    }
}
