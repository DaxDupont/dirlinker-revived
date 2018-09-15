using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DirLinker.Interfaces;
using DirLinker.Tests.Helpers;
using DirLinker.Commands;

namespace DirLinker.Tests.Commands
{
    [TestFixture]
    public class CreateFileLinkCommandTests
    {
        [Test]
        public void Execute_ValidPaths_PassedToCommandCorrectly()
        {
            FakeFile linkTo = new FakeFile("linkTo");
            FakeFile linkFrom = new FakeFile("linkFrom");

            ICommand linkFileCommand = new CreateFileLinkCommand(linkTo, linkFrom);

            linkFileCommand.Execute();

            Assert.AreSame("linkTo", linkFrom.FileLinkCreatedAt);
        }

        [Test]
        public void Undo_NoExceptionThrown()
        {
            FakeFile linkTo = new FakeFile("linkTo");
            FakeFile linkFrom = new FakeFile("linkFrom");

            ICommand linkFileCommand = new CreateFileLinkCommand(linkTo, linkFrom);

            Assert.DoesNotThrow(() => linkFileCommand.Undo());

        }

        [Test]
        public void UserFeedback_ValidStringReturned()
        {
            FakeFile linkTo = new FakeFile("linkTo");
            FakeFile linkFrom = new FakeFile("linkFrom");

            ICommand linkFileCommand = new CreateFileLinkCommand(linkTo, linkFrom);

            Assert.IsFalse(String.IsNullOrEmpty(linkFileCommand.UserFeedback));

        }

    }
}
