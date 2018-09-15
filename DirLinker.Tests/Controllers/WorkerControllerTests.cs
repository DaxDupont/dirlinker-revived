using System;
using Rhino.Mocks;
using NUnit.Framework;
using DirLinker.Interfaces;
using DirLinker.Controllers;
using DirLinker.Interfaces.Views;
using System.Windows.Threading;
using DirLinker.Data;
using System.Windows.Forms;
using DirLinker.Commands;

namespace DirLinker.Tests.Controllers
{
    [TestFixture]
    public class WorkerControllerTests
    {
        [Test]
        public void ShowWorker_ValidLinkService_DispatcherIsSet()
        {
            var link = MockRepository.GenerateMock<ILinkerService>();
            var view = MockRepository.GenerateMock<IWorkingView>();
            
            var workerController = new WorkerController(link, view);

            workerController.ShowWorker(null);

            link.AssertWasCalled(l => l.GetStatusData(Arg<Dispatcher>.Is.NotNull));
        }

        [Test]
        public void ShowWorker_ValidLinkService_FeedbackDataIsSetInTheView()
        {
            FeedbackData data = new FeedbackData();
            var link = MockRepository.GenerateMock<ILinkerService>();
            link.Stub(l => l.GetStatusData(Arg<Dispatcher>.Is.Anything)).Return(data);

            var view = MockRepository.GenerateMock<IWorkingView>();


            var workerController = new WorkerController(link, view);

            workerController.ShowWorker(null);

            view.AssertWasCalled(v => v.Feedback = data);
        }


        [Test]
        public void ShowWorker_ValidView_ViewIsShown()
        {
            FeedbackData data = new FeedbackData();
            var link = MockRepository.GenerateMock<ILinkerService>();
            link.Stub(l => l.GetStatusData(Arg<Dispatcher>.Is.Anything)).Return(data);

            var view = MockRepository.GenerateMock<IWorkingView>();

            var workerController = new WorkerController(link, view);

            workerController.ShowWorker(null);

            view.AssertWasCalled(v => v.Show(Arg<IWin32Window>.Is.Anything));
        }

        [Test]
        public void ShowWorker_ValidLinkService_LinkOperationIsStarted()
        {
            FeedbackData data = new FeedbackData();
            var link = MockRepository.GenerateMock<ILinkerService>();
            link.Stub(l => l.GetStatusData(Arg<Dispatcher>.Is.Anything)).Return(data);

            var view = MockRepository.GenerateMock<IWorkingView>();

            var workerController = new WorkerController(link, view);

            workerController.ShowWorker(null);

            link.AssertWasCalled(l => l.PerformOperation());
        }

        [Test]
        public void ShowWorker_CancelIsPressedWhenViewIsShown_LinkerServiceIsRequestedToCancel()
        {
            FeedbackData data = new FeedbackData();
            var link = MockRepository.GenerateMock<ILinkerService>();
            link.Stub(l => l.GetStatusData(Arg<Dispatcher>.Is.Anything)).Return(data);
            
            var view = MockRepository.GenerateMock<IWorkingView>();
            view.Stub(v => v.Show(Arg<IWin32Window>.Is.Anything))
                                                    .Do((Action<IWin32Window>)delegate(IWin32Window owner) 
                                                    {
                                                        view.GetEventRaiser(v => v.CancelPress += null).Raise(null, null);
                                                    });

            var controller = new WorkerController(link, view);
            controller.ShowWorker(null);

            link.AssertWasCalled(l => l.CancelOperation());
        }


        [Test]
        public void ShowWorker_ViewIsShown_CancelButtonTextIsSetToCancel()
        {
            FeedbackData data = new FeedbackData();
            var link = MockRepository.GenerateMock<ILinkerService>();
            link.Stub(l => l.GetStatusData(Arg<Dispatcher>.Is.Anything)).Return(data);

            var view = MockRepository.GenerateMock<IWorkingView>();

            var controller = new WorkerController(link, view);
            controller.ShowWorker(null);

            view.AssertWasCalled(v => v.CancelButtonText = "Cancel");
            
        }

        [Test]
        public void ShowWorker_OperationCompleteCallBackMade_CancelButtonTextIsSetToFinish()
        {
            FeedbackData data = new FeedbackData();
            var link = MockRepository.GenerateMock<ILinkerService>();
            link.Stub(l => l.GetStatusData(Arg<Dispatcher>.Is.Anything)).Return(data);
            link.Stub(l => l.OperationComplete = null).PropertyBehavior();
            link.Stub(l => l.PerformOperation())
                .Do((Action)delegate
                {
                    link.OperationComplete();
                });

            var view = MockRepository.GenerateMock<IWorkingView>();

            var controller = new WorkerController(link, view);
            controller.ShowWorker(null);

            view.AssertWasCalled(v => v.CancelButtonText = "Finish");

        }

        [Test]
        public void ShowWorker_OperationCompleleCallBackNotMade_CancelButtonTextIsNotSetToFinish()
        {
            FeedbackData data = new FeedbackData();
            var link = MockRepository.GenerateMock<ILinkerService>();
            link.Stub(l => l.GetStatusData(Arg<Dispatcher>.Is.Anything)).Return(data);
            link.Stub(l => l.OperationComplete = null).PropertyBehavior();
     

            var view = MockRepository.GenerateMock<IWorkingView>();

            var controller = new WorkerController(link, view);
            controller.ShowWorker(null);

            view.AssertWasNotCalled(v => v.CancelButtonText = "Finish");

        }

        [Test]
        public void ShowWorker_OperationIsCompleleCancelButtonPressed_NoCancelIsCalledOnServiceViewIsClosed()
        {
            FeedbackData data = new FeedbackData();
            var link = MockRepository.GenerateMock<ILinkerService>();
            link.Stub(l => l.GetStatusData(Arg<Dispatcher>.Is.Anything)).Return(data);
            link.Stub(l => l.OperationComplete = null).PropertyBehavior();
            link.Stub(l => l.PerformOperation())
             .Do((Action)delegate
             {
                 link.OperationComplete();
             });

            var view = MockRepository.GenerateMock<IWorkingView>();

            var controller = new WorkerController(link, view);
            controller.ShowWorker(null);

            view.GetEventRaiser(v => v.CancelPress += null).Raise(null, null);

            link.AssertWasNotCalled(l => l.CancelOperation());
            view.AssertWasCalled(v => v.Close());
        }
    }
}
