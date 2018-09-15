using System;
using System.Windows.Forms;
using DirLinker.Data;

namespace DirLinker.Interfaces.Views
{
    public interface IWorkingView : IWin32Window
    {
        FeedbackData Feedback { set; }
        String CancelButtonText { set; }
        event EventHandler CancelPress;

        void Show(IWin32Window owner);
        void Close();
    }
}
