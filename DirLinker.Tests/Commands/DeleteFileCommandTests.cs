using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DirLinker.Interfaces;
using DirLinker.Tests.Helpers;
using DirLinker.Commands;
using System.IO;

namespace DirLinker.Tests.Commands
{
    [TestFixture]
    public class DeleteFileCommandTests
    {
        [Test]
        public void Execute_FileIsNotReadOnly_FileDeleted()
        {
            var fileToDelete = new FakeFile("fileToDelete");
            fileToDelete.ExistsReturnValue = true;
            var deleteCommand = new DeleteFileCommand(fileToDelete);

            deleteCommand.Execute();

            Assert.IsTrue(fileToDelete.DeleteCalled);
        }

        [Test]
        public void Execute_FileIsReadOnly_ReadonlyCleared()
        {
            var fileToDelete = new FakeFile("fileToDelete");
            fileToDelete.ExistsReturnValue = true;
            fileToDelete.Attributes = FileAttributes.ReadOnly;

            var deleteCommand = new DeleteFileCommand(fileToDelete);

            deleteCommand.Execute();

            Assert.AreEqual(FileAttributes.Normal, fileToDelete.Attributes);
        }

        [Test]
        public void Execute_FileDoesNotExist_NoFileDeleteIsCalled()
        {
            var fileToDelete = new FakeFile("fileToDelete");
            fileToDelete.ExistsReturnValue = false;
            fileToDelete.Attributes = FileAttributes.ReadOnly;

            var deleteCommand = new DeleteFileCommand(fileToDelete);

            deleteCommand.Execute();

            Assert.IsFalse(fileToDelete.DeleteCalled);
        }

        [Test]
        public void Undo_DoesNothing()
        {
            var fileToDelete = new FakeFile("fileToDelete");
            fileToDelete.Attributes = FileAttributes.ReadOnly;

            var deleteCommand = new DeleteFileCommand(fileToDelete);

            Assert.DoesNotThrow(() => deleteCommand.Undo());
        }

        [Test]
        public void UserFeedback_ReturnsStringWithContent()
        {
            var fileToDelete = new FakeFile("fileToDelete");
            fileToDelete.Attributes = FileAttributes.ReadOnly;

            var deleteCommand = new DeleteFileCommand(fileToDelete);
            Assert.IsFalse(String.IsNullOrEmpty(deleteCommand.UserFeedback));
        }


    }
}
