using System;
using NUnit.Framework;
using DirLinker.Interfaces;
using DirLinker.Tests.Helpers;
using DirLinker.Commands;

namespace DirLinker.Tests.Commands
{
    [TestFixture]
    public class CreateFolderCommandTests
    {
        [Test]
        public void Execute_Creates_Folder()
        {
            FakeFolder folder = new FakeFolder(@"C:\testFolder");

            ICommand createFolderComand = new CreateFolderCommand(folder);
            createFolderComand.Execute();

            Assert.IsTrue(folder.CreateFolderCalled);
        }

        [Test]
        public void Execute_folder_exists_no_create_called()
        {
            FakeFolder folder = new FakeFolder(@"C:\testFolder");
            folder.FolderExistsReturnValue = true;

            ICommand createFolderComand = new CreateFolderCommand(folder);
            createFolderComand.Execute();

            Assert.IsFalse(folder.CreateFolderCalled);
        }

        [Test]
        public void Undo_created_folder_deleted()
        {
            FakeFolder folder = new FakeFolder(@"C:\testFolder");

            ICommand createFolderComand = new CreateFolderCommand(folder);

            createFolderComand.Execute();
            createFolderComand.Undo();

            Assert.IsTrue(folder.DeleteFolderCalled);
        }

        [Test]
        public void Undo_no_folder_created_no_delete_attempted()
        {
            FakeFolder folder = new FakeFolder(@"C:\testFolder");

            ICommand createFolderComand = new CreateFolderCommand(folder);
            createFolderComand.Undo();

            Assert.IsFalse(folder.DeleteFolderCalled);
        }

        [Test]
        public void Undo_folder_existed_before_execute_no_delete_attempted()
        {
            FakeFolder folder = new FakeFolder(@"C:\testFolder");
            folder.FolderExistsReturnValue = true;

            ICommand createFolderComand = new CreateFolderCommand(folder);

            createFolderComand.Execute();
            createFolderComand.Undo();

            Assert.IsFalse(folder.DeleteFolderCalled);
        }

        [Test]
        public void Status_return_status_with_current_folder_name_in()
        {
            String folderName = @"C:\testFolder";
            FakeFolder folder = new FakeFolder(folderName);
            folder.FolderExistsReturnValue = true;

            ICommand createFolderComand = new CreateFolderCommand(folder);
            String status = createFolderComand.UserFeedback;

            Assert.IsTrue(status.Contains(folderName));


        }


    }
}
