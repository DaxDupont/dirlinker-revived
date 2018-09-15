using System;
using NUnit.Framework;
using DirLinker.Tests.Helpers;
using DirLinker.Commands;
using DirLinker.Interfaces;
using Rhino.Mocks;
using System.IO;

namespace DirLinker.Tests.Commands
{
    [TestFixture]
    public class MoveFileCommandTests
    { 
        
        [Test]
        public void Status_should_return_status_with_both_filenames_in()
        {
            FakeFile sourceFile = new FakeFile("file1");
            FakeFile targetFile = new FakeFile("file2");

            MoveFileCommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, false);
            String status = testCopyCommand.UserFeedback;

            Assert.That(status.Contains("file1"));
            Assert.That(status.Contains("file2"));
        }

        [Test]
        public void Execute_source_file_should_move_to_target()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Folder).Return(@"c:\source\");
            sourceFile.Stub(s => s.FileName).Return("source");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(s => s.Folder).Return(@"c:\source\");
            targetFile.Stub(s => s.FileName).Return("target");
            targetFile.Stub(s => s.Exists()).Return(false);

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, false);
            testCopyCommand.Execute();

            sourceFile.AssertWasCalled(f => f.MoveFile(Arg<IFile>.Is.Same(targetFile)));
        }
        
        [Test]
        public void Execute_Target_File_Exists_When_Overwriting_Should_Check_Target_ReadOnly_Flag()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Folder).Return(@"c:\source");
            sourceFile.Stub(s => s.FileName).Return("File1");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(f => f.Exists()).Return(true);
            targetFile.Stub(f => f.GetAttributes()).Return(FileAttributes.Normal);
           
            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.Execute();

            targetFile.AssertWasCalled(f => f.GetAttributes());
        }

        [Test]
        public void Execute_Target_File_Readonly_User_Asked_To_Confirm()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Folder).Return(@"c:\source");
            sourceFile.Stub(s => s.FileName).Return("File1");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(f => f.Exists()).Return(true);
            targetFile.Stub(f => f.GetAttributes()).Return(FileAttributes.ReadOnly);

            Boolean responseRequested = false;
            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.AskUser += (question, options) =>
                {
                    responseRequested = true;
                    return System.Windows.Forms.DialogResult.OK;
                };

            testCopyCommand.Execute();

            Assert.IsTrue(responseRequested);
        }

        [Test]
        public void Execute_Target_File_Readonly_User_Yes_Read_Only_Cleared_File_Copied()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Folder).Return(@"c:\source");
            sourceFile.Stub(s => s.FileName).Return("File1");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(f => f.Exists()).Return(true);
            targetFile.Stub(f => f.GetAttributes()).Return(FileAttributes.ReadOnly);

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.AskUser += (question, options) =>
            {
                return System.Windows.Forms.DialogResult.Yes;
            };

            testCopyCommand.Execute();

            targetFile.AssertWasCalled(t => t.SetAttributes(FileAttributes.Normal));
            sourceFile.AssertWasCalled(s => s.MoveFile(targetFile));
        }

        [Test]
        public void Execute_Target_File_Readonly_User_No_Read_Only_Not_Cleared_File_Not_Copied()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Folder).Return(@"c:\source");
            sourceFile.Stub(s => s.FileName).Return("File1");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(f => f.Exists()).Return(true);
            targetFile.Stub(f => f.GetAttributes()).Return(FileAttributes.ReadOnly);

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.AskUser += (question, options) =>
            {
                return System.Windows.Forms.DialogResult.No;
            };

            testCopyCommand.Execute();

            targetFile.AssertWasNotCalled(t => t.SetAttributes(FileAttributes.Normal));
            sourceFile.AssertWasNotCalled(s => s.CopyFile(targetFile, true));
        }

        [Test]
        public void Execute_Target_File_Readonly_User_No_Read_Only_Not_Cleared_File_Is_Not_Deleted()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Folder).Return(@"c:\source");
            sourceFile.Stub(s => s.FileName).Return("File1");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(f => f.Exists()).Return(true);
            targetFile.Stub(f => f.GetAttributes()).Return(FileAttributes.ReadOnly);

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.AskUser += (question, options) =>
            {
                return System.Windows.Forms.DialogResult.No;
            };

            testCopyCommand.Execute();

            targetFile.AssertWasNotCalled(t => t.Delete());
        }


        [Test]
        public void Execute_target_file_exists_no_overwrite_copy_not_attempted()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Folder).Return(@"c:\source\");
            sourceFile.Stub(s => s.FileName).Return("source");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(s => s.Exists()).Return(true);
            targetFile.Stub(s => s.Folder).Return(@"c:\source\");
            targetFile.Stub(s => s.FileName).Return("target");

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, false);
            testCopyCommand.Execute();
            
            sourceFile.AssertWasNotCalled(f => f.CopyFile(Arg<IFile>.Is.Anything, Arg<Boolean>.Is.Anything));
        }

        [Test]
        public void Execute_target_file_exists_overwrite_target_should_be_overwritten()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Folder).Return(@"c:\source\");
            sourceFile.Stub(s => s.FileName).Return("source");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(s => s.Exists()).Return(true);
            targetFile.Stub(s => s.Folder).Return(@"c:\source\");
            targetFile.Stub(s => s.FileName).Return("target");

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.Execute();

            sourceFile.AssertWasCalled(f => f.MoveFile(targetFile));

        }

        [Test]
        public void Undo_Copies_target_back_to_source_when_source_does_not_exist()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Exists()).Return(false);
            sourceFile.Stub(s => s.Folder).Return(@"c:\source\");
            sourceFile.Stub(s => s.FileName).Return("source");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(s => s.Exists()).Return(true);
            targetFile.Stub(s => s.Folder).Return(@"c:\source\");
            targetFile.Stub(s => s.FileName).Return("target");

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.Execute();

            testCopyCommand.Undo();

            targetFile.AssertWasCalled(x => x.MoveFile(Arg<IFile>.Is.Same(sourceFile)));

        }

        [Test]
        public void Undo_Does_not_copy_target_to_source_if_source_exists()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Exists()).Return(true);
            sourceFile.Stub(s => s.Folder).Return(@"c:\source\");
            sourceFile.Stub(s => s.FileName).Return("source");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(s => s.Exists()).Return(true);
            targetFile.Stub(s => s.Folder).Return(@"c:\source\");
            targetFile.Stub(s => s.FileName).Return("target");

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.Execute();

            testCopyCommand.Undo();

            targetFile.AssertWasNotCalled(x => x.CopyFile(Arg<IFile>.Is.Same(sourceFile), Arg<Boolean>.Is.Anything));

        }

        [Test]
        public void Undo_Does_nothing_if_executed_has_not_been_called_first()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Exists()).Return(false);
            sourceFile.Stub(s => s.Folder).Return(@"c:\source\");
            sourceFile.Stub(s => s.FileName).Return("source");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(s => s.Exists()).Return(true);
            targetFile.Stub(s => s.Folder).Return(@"c:\source\");
            targetFile.Stub(s => s.FileName).Return("target");

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.Undo();

            targetFile.AssertWasNotCalled(t => t.CopyFile(Arg<IFile>.Is.Anything, Arg<Boolean>.Is.Anything));
        }

        [Test]
        public void Undo_Copy_success_deletes_target_file_when_undoing()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Exists()).Return(false);
            sourceFile.Stub(s => s.Folder).Return(@"c:\source\");
            sourceFile.Stub(s => s.FileName).Return("source");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(s => s.Exists()).Return(true);
            targetFile.Stub(s => s.Folder).Return(@"c:\source\");
            targetFile.Stub(s => s.FileName).Return("target");

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.Execute();
            testCopyCommand.Undo();

            targetFile.AssertWasCalled(t => t.Delete());
        }

        [Test]
        public void Execute_target_file_exists_overwrite_specified_target_deleted()
        {
            IFile sourceFile = MockRepository.GenerateStub<IFile>();
            sourceFile.Stub(s => s.Exists()).Return(false);
            sourceFile.Stub(s => s.Folder).Return(@"c:\source\");
            sourceFile.Stub(s => s.FileName).Return("source");

            IFile targetFile = MockRepository.GenerateStub<IFile>();
            targetFile.Stub(s => s.Exists()).Return(true);
            targetFile.Stub(s => s.Folder).Return(@"c:\source\");
            targetFile.Stub(s => s.FileName).Return("target");

            ICommand testCopyCommand = new MoveFileCommand(sourceFile, targetFile, true);
            testCopyCommand.Execute();


            targetFile.AssertWasCalled(t => t.Delete());
        }

    }
}
