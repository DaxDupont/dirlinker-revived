using System;
using System.Windows.Threading;
using DirLinker.Data;
namespace DirLinker.Interfaces
{
    public delegate IMessenger ThreadMessengerFactory(Dispatcher dispatcher, FeedbackData data);
    public interface IMessenger
    {
        System.Windows.Forms.DialogResult RequestUserFeedback(string message, System.Windows.Forms.MessageBoxButtons options);
        void StatusUpdate(string message, int percentageComplete);
    }
}
