using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DirLinker.Interfaces;
using DirLinker.Commands;
using Rhino.Mocks;

namespace DirLinker.Tests.Commands
{
    [TestFixture]
    public class CreateLinkXPCommandTests
    {
        [Test]
        public void Execute_ValidValues_LinkCreated()
        {
            var junctionPoint = MockRepository.GenerateMock<IJunctionPointXp>();
            
            String linkTo = "path1";
            String linkFrom = "path2";

            ICommand linkCommand = new CreateLinkXpCommand(linkTo, linkFrom, junctionPoint);
            linkCommand.Execute();

            junctionPoint.AssertWasCalled(j => j.Create(linkTo, linkFrom));
        }

        [Test]
        public void Undo_DoesNothing()
        {
            ICommand linkCommand = new CreateLinkXpCommand(null, null, null);
            Assert.DoesNotThrow(() => linkCommand.Undo());

        }

        [Test]
        public void UserFeedback_ValidStringReturned()
        {
            ICommand linkCommand = new CreateLinkXpCommand("path", "path", null);
            String feedback = linkCommand.UserFeedback;

            Assert.IsFalse(String.IsNullOrEmpty(feedback));
        }
    }
}
