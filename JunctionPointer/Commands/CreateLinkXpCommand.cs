using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;

namespace DirLinker.Commands
{
    public class CreateLinkXpCommand : ICommand
    {
        private string _linkTo;
        private string _linkFrom;
        private IJunctionPointXp _junctionPoint;

        public CreateLinkXpCommand(string linkTo, string linkFrom, IJunctionPointXp junctionPoint)
        {
            _linkTo = linkTo;
            _linkFrom = linkFrom;
            _junctionPoint = junctionPoint;
        }

        public void Execute()
        {
            _junctionPoint.Create(_linkTo, _linkFrom);
        }

        public void Undo()
        {
            //Intentionally left blank
        }

        public string UserFeedback
        {
            get { return String.Format("Creating a link at {0}", _linkTo); }
        }
#pragma warning disable 00067
        public event RequestUserReponse AskUser;
#pragma warning restore 00067



    }
}
