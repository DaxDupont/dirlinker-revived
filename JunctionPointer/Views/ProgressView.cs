using System;
using System.Windows.Forms;
using DirLinker.Interfaces.Views;
using DirLinker.Data;

namespace DirLinker.Views
{
    public partial class ProgressView : Form, IWorkingView
    {
        private FeedbackData _data;
        private EventHandler _cancelHandler;

        public ProgressView()
        {
            InitializeComponent();
            FormClosing += FormClosingEvent;
        }

        void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            var cancelHander = _cancelHandler;

            if (cancelHander != null && cancelBtn.Text.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
            {
                cancelHander(this, new EventArgs());
                e.Cancel = true;
            }
        }
        
        public FeedbackData Feedback
        {
            set 
            { 
                _data = value; 
                BindToData(); 
            }
        }

        private void BindToData()
        {
            progressBar1.DataBindings.Add("Value", _data, "PercentageComplete", false, DataSourceUpdateMode.OnPropertyChanged);
            _data.PropertyChanged += (s, ea) =>
                                        {
                                            if (ea.PropertyName.Equals("Message"))
                                                textBox1.AppendText(_data.Message + Environment.NewLine);
                                        };

            _data.AskUser = (m) =>  MessageBox.Show(this, m.Message, "DirLinker", m.ResponseOptions);
             
        }

        public string CancelButtonText
        {
            set { cancelBtn.Text = value; }
        }

        public event EventHandler CancelPress
        {
            add
            {
                _cancelHandler = value;
                cancelBtn.Click += value;
            }
            remove
            {
                _cancelHandler = null;
                cancelBtn.Click -= value;
            }
        }

    }
}
