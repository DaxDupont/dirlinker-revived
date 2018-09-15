using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Exceptions;
using DirLinker.Interfaces;
using DirLinker.Data;
using System.Windows.Threading;
using DirLinker.Commands;
using System.Threading;
using System.Windows.Forms;

namespace DirLinker.Implementation
{
    public class LinkerService : ILinkerService
    {
        private ITransactionalCommandRunner _commandRunner;
        private Action _completeCallBack;
        private FeedbackData _feedback;
        private ICommandDiscovery _commandDiscovery;
        private ThreadMessengerFactory _messengerFactory;
        private LinkOperationData _operationData;

        public LinkerService(ICommandDiscovery commandDiscovery, ITransactionalCommandRunner runner, ThreadMessengerFactory messengerFactory)
        {
            _commandDiscovery = commandDiscovery;
            _commandRunner = runner;
            _messengerFactory = messengerFactory;
        }
    
        public FeedbackData GetStatusData(Dispatcher dispatcher)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }


            _feedback = new FeedbackData();
            return _feedback;
        }

        
        public void PerformOperation()
        {
            try
            {
                QueueCommands();

                RunCommandRunner();
            }
            catch (DirLinkerException ex)
            {
                _feedback.AskUser(new FeedbackData.UserMessage()
                                      {
                                          Message = String.Format("Unable to Perform Operation: {0} {1}", Environment.NewLine, ex.Message), 
                                          ResponseOptions = MessageBoxButtons.OK
                                      });

                OnCommandRunnerOnWorkCompleted(new WorkReport(WorkStatus.CommandFailWithException, ex));

            }
        }

        
        public void CancelOperation()
        {
            _commandRunner.RequestCancel();
        }

        public void SetOperationData(LinkOperationData linkData)
        {
            _operationData = linkData;
        }

        private void QueueCommands()
        {
           
                UpdateFeedBack("Building Task List");

                var commandList = _commandDiscovery.GetCommandListTask(
                    _operationData.LinkTo,
                    _operationData.CreateLinkAt,
                    _operationData.CopyBeforeDelete,
                    _operationData.OverwriteExistingFiles);

                _commandRunner.QueueRange(commandList);
          

        }

        private void RunCommandRunner()
        {
            _commandRunner.WorkCompleted += OnCommandRunnerOnWorkCompleted;

            _commandRunner.RunAsync(_messengerFactory(Dispatcher.CurrentDispatcher, _feedback));

        }

        private void OnCommandRunnerOnWorkCompleted(WorkReport wr)
        {
            UpstatusFromReport(wr);
            if (_completeCallBack != null)
            {
                _completeCallBack();
            }

            _commandRunner.WorkCompleted -= OnCommandRunnerOnWorkCompleted;
        }


        private void UpstatusFromReport(WorkReport wr)
        {
            switch (wr.Status)
            {
                case WorkStatus.Success:
                    UpdateFeedBack("Completed successfully");
                    break;
                case WorkStatus.UserCancelled:
                    UpdateFeedBack("User cancelled");
                    break;
                case WorkStatus.CommandFailWithException:
                    UpdateFeedBack("Operation failed");
                    break;
                case WorkStatus.UndoFailWithException:
                    UpdateFeedBack("Undo failed");
                    break;
            }
        }

        private void UpdateFeedBack(String message)
        {
            if (_feedback != null)
            {
                _feedback.Message = message;
                _feedback.PercentageComplete = 100;
            }
        }

        public Action OperationComplete
        {
            set { _completeCallBack = value; }
            get
            {
                return _completeCallBack;
            }
        }

    }
}
