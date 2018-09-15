using System.Collections.Generic;
using NUnit.Framework;
using DirLinker.Tests.Helpers;
using DirLinker.Interfaces;
using DirLinker.Commands;
using Rhino.Mocks;

namespace DirLinker.Tests.Commands
{
    [TestFixture]
    public class DeleteFolderCommandTests
    {
        [Test]
        public void Execute_Folder_deleted()
        {
            FakeFolder folder = new FakeFolder(@"c:\fakeFolder\");
            folder.FolderExistsReturnValue = true;

            ICommand deleteCommand = new DeleteFolderCommand(folder);
            deleteCommand.Execute();

            Assert.IsTrue(folder.DeleteFolderCalled);
        }

        [Test]
        public void Undo_Creates_folder_when_undo_is_called_after_execute()
        {
            FakeFolder folder = new FakeFolder(@"c:\fakeFolder\");
            folder.FolderExistsReturnValue = true;

            ICommand deleteCommand = new DeleteFolderCommand(folder);
            deleteCommand.Execute();
            deleteCommand.Undo();

            Assert.IsTrue(folder.CreateFolderCalled);
        }

        [Test]
        public void Undo_Execute_not_called_folder_is_not_created()
        {
            FakeFolder folder = new FakeFolder(@"c:\fakeFolder\");
            folder.FolderExistsReturnValue = true;

            ICommand deleteCommand = new DeleteFolderCommand(folder);
            deleteCommand.Undo();

            Assert.IsFalse(folder.CreateFolderCalled);
        }

        [Test]
        public void Execute_folder_does_not_exist_delete_not_called()
        {
            FakeFolder folder = new FakeFolder(@"c:\fakeFolder\");
            folder.FolderExistsReturnValue = false;

            ICommand deleteCommand = new DeleteFolderCommand(folder);
            deleteCommand.Execute();

            Assert.IsFalse(folder.DeleteFolderCalled);
        }

        [Test]
        public void UserFeedback_returns_a_status_containing_the_folder_name()
        {
            string folderPath = @"fakeFolder";
            FakeFolder folder = new FakeFolder(folderPath);
            folder.FolderExistsReturnValue = true;

            ICommand deleteCommand = new DeleteFolderCommand(folder);


            StringAssert.Contains(folderPath, deleteCommand.UserFeedback);
        }

        [Test]
        public void Execute_Folder_contains_files_files_delete_first()
        {
            FakeFolder folder = new FakeFolder(@"fakeFolder");
            folder.FolderExistsReturnValue = true;
            folder.FileList = new List<IFile>() {
                                                new FakeFile("file1"),
                                                new FakeFile("file2"),
                                                };

            ICommand deleteComand = new DeleteFolderCommand(folder);
            deleteComand.Execute();

            Assert.IsTrue(folder.FileList.TrueForAll(f => ((FakeFile)f).DeleteCalled), "Not all the files were deleted before trying to delete the folder!");

        }

        [Test]
        public void Execute_folder_contains_readonly_files_readonly_attribute_cleared()
        {
            FakeFolder folder = new FakeFolder(@"fakeFolder");
            folder.FolderExistsReturnValue = true;
            folder.FileList = new List<IFile>() {
                                                new FakeFile("file1") { Attributes = System.IO.FileAttributes.ReadOnly },
                                                new FakeFile("file2") { Attributes = System.IO.FileAttributes.ReadOnly },
                                                };

            ICommand command = new DeleteFolderCommand(folder);
            command.Execute();
            Assert.IsTrue(folder.FileList.TrueForAll(f => ((FakeFile)f).Attributes == System.IO.FileAttributes.Normal));
        }

    
        [Test]
        public void Execute_FolderContainsSubFolders_SubFolderDeleted()
        {
            var folder1 = MockRepository.GenerateMock<IFolder>();
            folder1.Stub(f => f.GetSubFolderList()).Return(new List<IFolder>());
            folder1.Stub(f => f.GetFileList()).Return(new List<IFile>());

            var folder2 = MockRepository.GenerateMock<IFolder>();
            folder2.Stub(f => f.GetSubFolderList()).Return(new List<IFolder>());
            folder2.Stub(f => f.GetFileList()).Return(new List<IFile>());

            FakeFolder folder = new FakeFolder(@"fakeFolder");
            folder.FolderExistsReturnValue = true;
            folder.SubFolderList = new List<IFolder>() {
                                                folder1,
                                                folder2,
                                                };

            ICommand deleteComand = new DeleteFolderCommand(folder);

            deleteComand.Execute();

            folder1.AssertWasCalled(f => f.DeleteFolder());
            folder2.AssertWasCalled(f => f.DeleteFolder());

        }

        [Test]
        public void Execute_FolderContainsSubFoldersWithFiles_SubFolderFilesDeleted()
        {
            var folder1 = MockRepository.GenerateMock<IFolder>();
            folder1.Stub(f => f.GetSubFolderList()).Return(new List<IFolder>());
            var file = MockRepository.GenerateMock<IFile>();

            folder1.Stub(f => f.GetFileList()).Return(new List<IFile>() { file });

            FakeFolder folder = new FakeFolder(@"fakeFolder");
            folder.FolderExistsReturnValue = true;
            folder.SubFolderList = new List<IFolder>() {
                                                folder1,
                                                };

            ICommand deleteComand = new DeleteFolderCommand(folder);

            deleteComand.Execute();

            file.AssertWasCalled(f => f.Delete());

        }

       
    }
}
