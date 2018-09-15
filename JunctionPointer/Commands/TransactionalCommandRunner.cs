using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.ComponentModel;
using DirLinker.Interfaces;
using DirLinker.Implementation;
using System.Windows.Forms;
using DirLinker.Data;

namespace DirLinker.Commands
{
     /// <summary>
     /// This is delegate called when an operation has completed
     /// </summary>
     /// <param name="report">A summary of the work performed</param>
    public delegate void WorkCompletedCallBack(WorkReport report);
    /// <summary>
    /// 
    /// </summary>
    public class TransactionalCommandRunner :  ITransactionalCommandRunner
    {
        private Stack<ICommand> _undoStack;
        private ThreadSafeQueue<ICommand> _commandQueue;
        private IBackgroundWorker _bgWorker;

        private Boolean _cancelRequested;

        private WorkCompletedCallBack _workCompleted;
        private WorkReportGenerator _workReportCreator;

        public TransactionalCommandRunner(IBackgroundWorker backgroundWorker, ThreadSafeQueue<ICommand> queue)
        {
            _commandQueue = queue;
            _bgWorker = backgroundWorker;
            _undoStack = new Stack<ICommand>();
            _workReportCreator = new WorkReportGenerator();

            _cancelRequested = false;
        }

        /// <summary>
        /// This event is raised when all the work has been completed.  It will return a 
        /// a complete work report including any exceptions. 
        /// </summary>
        public event WorkCompletedCallBack WorkCompleted
        {
            add { _workCompleted += value; }
            remove { _workCompleted -= value; }
        }

        /// <summary>
        /// Returns the current number of commands in the queue
        /// </summary>
        public Int32 CommandQueueCount
        {
            get
            {
                return _commandQueue.Count;
            }
        }

        /// <summary>
        /// Adds a command to the queue to be run
        /// </summary>
        /// <param name="command">a class that implements ICommand</param>
        public void QueueCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command", "command is null.");
            }

            _commandQueue.Enqueue(command);
        }

        public void QueueRange(List<ICommand> commandList)
        {
            commandList.ForEach(c => QueueCommand(c));
        }

        /// <summary>
        /// Starts a background thread and runs the current command queue.
        /// When finished The WorkCompleted event will be raised.
        /// </summary>
        public void RunAsync(IMessenger messenger)
        {
            _bgWorker.DoWork += RunWorker;

            _bgWorker.RunWorkerCompleted += NotifyWorkCompleted;
            _bgWorker.RunWorkerAsync(messenger); 
        }


        private void RunWorker(object sender, DoWorkEventArgs args)
        {
            RunCommandQueue(args.Argument as IMessenger);
        }
        /// <summary>
        /// Requests the work is cancelled.  The cancel flag is only checked between running commands.
        /// </summary>
        /// 
        public void RequestCancel()
        {
            _cancelRequested = true;
            _workReportCreator.UserCancelledRequested();
        }

        private void RunCommandQueue(IMessenger messenger)
        {
            try
            {
                Int32 commandsExe = 1;
                Int32 totalNumber = _commandQueue.Count;
                foreach (ICommand command in _commandQueue.ProcessQueue())
                {
                    if (_cancelRequested)
                    {
                        AttemptRollBack(messenger, "User requested cancel");
                        break;
                    }

                    messenger.StatusUpdate(command.UserFeedback, PercentageComplete(commandsExe, totalNumber));

                    command.AskUser += messenger.RequestUserFeedback;

                    command.Execute();
                    _undoStack.Push(command);

                    command.AskUser -= messenger.RequestUserFeedback;

                    commandsExe++;
                }
            }
            catch (Exception ex)
            {
                _workReportCreator.ProcessException(ex, WorkAction.Execute);
                AttemptRollBack(messenger, String.Format("An error occured: {0}", ex.Message));
            }
        }

        private Int32 PercentageComplete(int commandsExe, int totalNumber)
        {
            Int32 percentage = (commandsExe * 100) / totalNumber;
            return percentage;
        }

        private void AttemptRollBack(IMessenger messenger, String reason)
        {
            var res = messenger.RequestUserFeedback( String.Format("{0}. Do you want to undo any changes made?", reason), MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                messenger.StatusUpdate("An Error occured, attemping to rollback changes", 0);

                ProcessUndoStack(messenger);
            }
        }

        private void ProcessUndoStack(IMessenger messenger)
        {
            try
            {
                Int32 commandsExe = 1;
                Int32 totalNumber = _undoStack.Count;

                while (_undoStack.Count > 0)
                {
                    ICommand command = _undoStack.Pop();

                    messenger.StatusUpdate(String.Format("Undoing: {0}", command.UserFeedback), PercentageComplete(commandsExe, totalNumber));

                    command.Undo();
                    commandsExe++;
                }
            }
            catch (Exception ex)
            {
                _workReportCreator.ProcessException(ex, WorkAction.Undo);

                messenger.StatusUpdate("An Error occured when rolling back changes", 0);
            }
        }


        private void NotifyWorkCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            WorkCompletedCallBack workCompletedCopy = _workCompleted;
            if (workCompletedCopy != null)
            {
                workCompletedCopy(_workReportCreator.GenerateReport());
            }

            _bgWorker.RunWorkerCompleted -= NotifyWorkCompleted;
            _bgWorker.DoWork -= RunWorker;

            _cancelRequested = false;
            _undoStack.Clear();
            _commandQueue.Clear();
        }
    }
}
