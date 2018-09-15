using System;
using NUnit.Framework;
using DirLinker.Implementation;
using DirLinker.Data;
using System.Windows.Threading;
using Rhino.Mocks;
using DirLinker.Interfaces;
using DirLinker.Tests.Helpers;
using DirLinker.Commands;
using System.Collections.Generic;

namespace DirLinker.Tests.Implementation
{
    [TestFixture]
    class LinkerServiceTests
    {
        [Test]
        public void GetStatusData_ValidMessengerPassed_StatusDataObjectIsReturned()
        {
            var linker = GetLinkerService();

            FeedbackData data = linker.GetStatusData(Dispatcher.CurrentDispatcher);

            Assert.IsNotNull(data);
        }

        [Test]
        public void GetStatusData_NullPassed_ExceptionThrown()
        {
            var linker = GetLinkerService();
            Assert.Throws<ArgumentNullException>(() => linker.GetStatusData(null));
        }

        [Test]
        public void PerformOperation_ValidData_CommandIsRequestedFromCommandDiscovery()
        {
            String linkTo = "Test2";
            String linkFrom = "Test1";

            var commandDiscovery = MockRepository.GenerateMock<ICommandDiscovery>();
            var linker = GetLinkerService(commandDiscovery);

            LinkOperationData data = new LinkOperationData { CopyBeforeDelete = true, CreateLinkAt = linkFrom, LinkTo = linkTo, OverwriteExistingFiles = true };

            linker.SetOperationData(data);

            linker.PerformOperation();

            commandDiscovery.AssertWasCalled(d => d.GetCommandListTask(
                                                  Arg<String>.Is.Equal(linkTo),
                                                  Arg<String>.Is.Equal(linkFrom), 
                                                  Arg<Boolean>.Matches(b => data.CopyBeforeDelete),
                                                  Arg<Boolean>.Matches(b => data.OverwriteExistingFiles)));

        }


        [Test]
        public void PerformOperation_ValidDataAndDispatcher_StatusSetToCommandDiscoveryRunning()
        {
            var commandDiscovery = MockRepository.GenerateMock<ICommandDiscovery>();
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
                       
            var linker = GetLinkerService(commandDiscovery, runner);
            var statusData = linker.GetStatusData(Dispatcher.CurrentDispatcher);
            LinkOperationData data = new LinkOperationData { CopyBeforeDelete = true, CreateLinkAt = "", LinkTo = "", OverwriteExistingFiles = true };

            linker.SetOperationData(data);

            Boolean statusSet = false;
            statusData.PropertyChanged += (s, ea) => statusSet |= ea.PropertyName.Equals("Message") && statusData.Message.Equals("Building Task List");

            linker.PerformOperation();

            Assert.IsTrue(statusSet);
        }

        [Test]
        public void PerformOperation_ValidData_CommandsQueuedInCommandRunnerFromCommandDiscovery()
        {
            List<ICommand> commandList = new List<ICommand>();
            var commandDiscovery = MockRepository.GenerateMock<ICommandDiscovery>();
            commandDiscovery.Stub(c => c.GetCommandListTask(Arg<String>.Is.Anything, Arg<String>.Is.Anything, Arg<Boolean>.Is.Anything, Arg<Boolean>.Is.Anything))
                .Return(commandList);

            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(commandDiscovery, runner);

            LinkOperationData data = new LinkOperationData { CopyBeforeDelete = true, CreateLinkAt = "", LinkTo = "", OverwriteExistingFiles = true };

            linker.SetOperationData(data);

            linker.PerformOperation();

            runner.AssertWasCalled(r => r.QueueRange(commandList));
        }

        
        [Test]
        public void PerformOperation_ValidData_CommandRunnerStartedWithValidThreadMessenger()
        {
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(runner);

            linker.SetOperationData(new LinkOperationData());
            linker.PerformOperation();

            runner.AssertWasCalled(r => r.RunAsync(Arg<IMessenger>.Is.NotNull));
        }

        [Test]
        public void PerformOperation_ValidData_CompletedCallBackIsRegistered()
        {
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(runner);

            linker.SetOperationData(new LinkOperationData());
            linker.PerformOperation();

            runner.AssertWasCalled(r => r.WorkCompleted += Arg<WorkCompletedCallBack>.Is.Anything);
        }

        [Test]
        public void CancelOperation_CancelRequested_CancelPassedToTestRunner()
        {
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(runner);

            linker.CancelOperation();
            
            runner.AssertWasCalled(r => r.RequestCancel());
        }


        [Test]
        public void OperationComplete_CallBackRegistered_CalledWhenComplete()
        {
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(runner);
            runner.Stub(r => r.RunAsync(Arg<IMessenger>.Is.Anything)).Do((Action<IMessenger>)delegate(IMessenger m) { runner.GetEventRaiser(r => r.WorkCompleted += null).Raise(new WorkReport(WorkStatus.NotSet));
            });

            Boolean called = false;
            linker.OperationComplete = () => called = true;

            linker.SetOperationData(new LinkOperationData());
            linker.PerformOperation();

            Assert.IsTrue(called);
        }

        [Test]
        public void OperationComplete_WorkReportSuccess_StatusSetToCompleteSuccess()
        {
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(runner);
            runner.Stub(r => r.RunAsync(Arg<IMessenger>.Is.Anything)).Do((Action<IMessenger>)delegate(IMessenger m)
            {
                runner.GetEventRaiser(r => r.WorkCompleted += null).Raise(new WorkReport(WorkStatus.Success));
            });
            
            var status = linker.GetStatusData(Dispatcher.CurrentDispatcher);
            linker.SetOperationData(new LinkOperationData());
            linker.PerformOperation();

            StringAssert.Contains("Completed", status.Message);
            StringAssert.Contains("success", status.Message);
        }

        [Test]
        public void OperationComplete_WorkReportCancel_StatusSetToUserCancel()
        {
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(runner);
            runner.Stub(r => r.RunAsync(Arg<IMessenger>.Is.Anything)).Do((Action<IMessenger>)delegate(IMessenger m)
            {
                runner.GetEventRaiser(r => r.WorkCompleted += null).Raise(new WorkReport(WorkStatus.UserCancelled));
            });

            var status = linker.GetStatusData(Dispatcher.CurrentDispatcher);
            linker.SetOperationData(new LinkOperationData());
            linker.PerformOperation();

            StringAssert.Contains("User", status.Message);
            StringAssert.Contains("cancel", status.Message);
        }

        [Test]
        public void OperationComplete_WorkReportCommandFailed_StatusSetToFailed()
        {
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(runner);
            runner.Stub(r => r.RunAsync(Arg<IMessenger>.Is.Anything)).Do((Action<IMessenger>)delegate(IMessenger m)
            {
                runner.GetEventRaiser(r => r.WorkCompleted += null).Raise(new WorkReport(WorkStatus.CommandFailWithException));
            });

            var status = linker.GetStatusData(Dispatcher.CurrentDispatcher);
            linker.SetOperationData(new LinkOperationData());
            linker.PerformOperation();

            StringAssert.Contains("failed", status.Message);
        }

        [Test]
        public void OperationComplete_WorkReportUndoFailed_StatusSetToUnoFailed()
        {
            var runner = MockRepository.GenerateMock<ITransactionalCommandRunner>();
            var linker = GetLinkerService(runner);
            runner.Stub(r => r.RunAsync(Arg<IMessenger>.Is.Anything)).Do((Action<IMessenger>)delegate(IMessenger m)
            {
                runner.GetEventRaiser(r => r.WorkCompleted += null).Raise(new WorkReport(WorkStatus.UndoFailWithException));
            });

            var status = linker.GetStatusData(Dispatcher.CurrentDispatcher);
            linker.SetOperationData(new LinkOperationData());
            linker.PerformOperation();

            StringAssert.Contains("Undo", status.Message);
            StringAssert.Contains("failed", status.Message);
        }

        private LinkerService GetLinkerService()
        {
            var commandDiscovery = MockRepository.GenerateMock<ICommandDiscovery>();
            return GetLinkerService(commandDiscovery);
        }

        private LinkerService GetLinkerService(ICommandDiscovery commandDiscovery, ITransactionalCommandRunner runner)
        {
            return GetLinkerService(commandDiscovery, runner, f => new FakeFolder(f));
        }

        private LinkerService GetLinkerService(ICommandDiscovery commandDiscovery)
        {
            var runner = MockRepository.GenerateStub<ITransactionalCommandRunner>();
            return GetLinkerService(commandDiscovery, runner, f => new FakeFolder(f));
        }

        private LinkerService GetLinkerService(ITransactionalCommandRunner runner)
        {
            var commandDiscovery = MockRepository.GenerateStub<ICommandDiscovery>();
            return GetLinkerService(commandDiscovery, runner, f => new FakeFolder(f));
        }

        private LinkerService GetLinkerService(ICommandDiscovery commandDiscovery,  ITransactionalCommandRunner runner, IFolderFactoryForPath folderFactory)
        {
            return new LinkerService(commandDiscovery, runner, (d, fd) => MockRepository.GenerateStub<IMessenger>());
        }

    }
}
