using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DirLinker.Commands;
using DirLinker.Interfaces;
using Rhino.Mocks;
using DirLinker.Exceptions;
using DirLinker.Tests.Helpers;

namespace DirLinker.Tests.Commands
{
    [TestFixture]
    public class CreateLinkCommandTests
    {
        [Test]
        public void Execute_link_is_created()
        {
            String linkToPath = @"testPath";
            IFolder linkTo = MockRepository.GenerateMock<IFolder>();
            linkTo.Stub(f => f.FolderExists()).Return(false);
            linkTo.Stub(f => f.FolderPath).Return(linkToPath);

            IFolder linkFrom = MockRepository.GenerateMock<IFolder>();
            linkTo.Stub(f => f.FolderExists()).Return(true);

            CreateFolderLinkCommand command = new CreateFolderLinkCommand(linkTo, linkFrom);

            command.Execute();

            linkFrom.AssertWasCalled(f => f.CreateLinkToFolderAt(linkToPath));                   
        }

        [Test]
        public void Execute_linkTo_folder_still_exists_exception_is_thrown()
        {
            IFolder linkTo = MockRepository.GenerateMock<IFolder>();
            linkTo.Stub(f => f.FolderExists()).Return(true);
            linkTo.Stub(f => f.FolderPath).Return(@"testPath");

            IFolder linkFrom = MockRepository.GenerateMock<IFolder>();
            linkTo.Stub(f => f.FolderExists()).Return(true);

            CreateFolderLinkCommand command = new CreateFolderLinkCommand(linkTo, linkFrom);

            Assert.Throws<DirLinkerException>( () => command.Execute());
        }

        [Test]
        public void Undo_throws_not_supported_exception()
        {
            CreateFolderLinkCommand command = new CreateFolderLinkCommand(null, null);

            Assert.Throws<NotSupportedException>(() => command.Undo());
        }

        [Test]
        public void Status_returns_a_status_method()
        {
            IFolder target = new FakeFolder(@"c:\dest\");
            IFolder source = new FakeFolder(@"c:\dest\");
            CreateFolderLinkCommand command = new CreateFolderLinkCommand(target, source);

            Assert.IsNotEmpty(command.UserFeedback);
        }
    }
}
