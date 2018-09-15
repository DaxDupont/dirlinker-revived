using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;
using DirLinker.Interfaces.Views;
using System.Windows.Forms;
using System.Windows.Threading;
using DirLinker.Data;

namespace DirLinker.Controllers
{

    public class WorkerController
    {
        private ILinkerService _linker;
        private IWorkingView _view;

        public WorkerController(ILinkerService linker, IWorkingView view)
        {
            _linker = linker;
            _view = view;
        }

        public void ShowWorker(IWin32Window owner)
        {
            SetupFeedback();
            SetupCancel();
            _view.Show(owner);

            _linker.OperationComplete = () => FinishOperation();
            _linker.PerformOperation(); 

        }

        private void FinishOperation()
        {
            _view.CancelButtonText = "Finish";
            _view.CancelPress -= CancelOperation;
            _view.CancelPress += (s, ea) => _view.Close();
            _linker.OperationComplete = null;
        }

        private void SetupCancel()
        {
            _view.CancelButtonText = "Cancel";
            _view.CancelPress += CancelOperation;
        }

        void CancelOperation(object sender, EventArgs e)
        {
            _linker.CancelOperation();
        }

        private void SetupFeedback()
        {
            FeedbackData feedback = _linker.GetStatusData(Dispatcher.CurrentDispatcher);
            _view.Feedback = feedback;
        }
    }
}
