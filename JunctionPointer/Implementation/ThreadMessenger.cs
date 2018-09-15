using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using DirLinker.Data;
using System.Windows.Forms;
using DirLinker.Interfaces;

namespace DirLinker.Implementation
{

    /// <summary>
    /// Safely marshalls messages across threads
    /// </summary>
    public class ThreadMessenger : IMessenger
    {
        private Dispatcher _Dispatcher;
        private FeedbackData _data;

        public ThreadMessenger(Dispatcher dispatch, FeedbackData data)
        {
            _data = data;
            _Dispatcher = dispatch;
        }

        public void StatusUpdate(String message, Int32 percentageComplete)
        {
            _Dispatcher.Invoke((Action)delegate
            {
                _data.Message = message;
                _data.PercentageComplete = percentageComplete;
            });
        }

        public DialogResult RequestUserFeedback(String message, MessageBoxButtons options)
        {
            return (DialogResult)_Dispatcher.Invoke((Func<DialogResult>)delegate
            {
                return _data.AskUser(new FeedbackData.UserMessage()
                {
                    Message = message,
                    ResponseOptions = options
                });
            });
        }
    }

}
