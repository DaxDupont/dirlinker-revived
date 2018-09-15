using System;
using System.Diagnostics;
using System.Windows.Forms;
using DirLinker.Data;
using DirLinker.Interfaces.Views;

namespace DirLinker.Views
{
    public partial class DirLinkerView : Form, ILinkerView
    {
        private LinkOperationData _linkOperationData;

        public DirLinkerView()
        {
            InitializeComponent();
            RegisterHandlers();
        }

        public string LinkPoint { get; set; }

        public string LinkTo { get; set; }

        public bool CopyBeforeDelete { get; set; }

        public bool OverWriteTargetFiles { get; set; }

        #region ILinkerView Members

        public event PerformLink PerformOperation
        {
            add { m_PerformOperation += value; }
            remove { m_PerformOperation -= value; }
        }

        public event PathValidater ValidatePath
        {
            add { m_ValidatePath += value; }
            remove { m_ValidatePath -= value; }
        }

        public Form MainForm
        {
            get { return this; }
        }

        public void SetOperationData(LinkOperationData data)
        {
            _linkOperationData = data;
            BindToFields();

            //Set up defaults
            _linkOperationData.CopyBeforeDelete = true;
        }

        public Func<Boolean> ValidOperation { get; set; }

        public void ShowMesage(string message)
        {
            MessageBox.Show(this, message, "Directory Linker");
        }

        #endregion

        protected event PathValidater m_ValidatePath;

        protected event PerformLink m_PerformOperation;


        /// <summary>
        /// Triggers the ValidatePath event.
        /// </summary>
        public virtual void CallValidatePath(ValidationArgs ea)
        {
            if (m_ValidatePath != null)
                m_ValidatePath(this, ea);
        }

        /// <summary>
        /// Triggers the PerformOperation event.
        /// </summary>
        public virtual void CallPerformOperation(EventArgs ea)
        {
            if (m_PerformOperation != null)
                m_PerformOperation(this, ea);
        }


        private void BindToFields()
        {
            LinkFrom.DataBindings.Add("Text", _linkOperationData, "CreateLinkAt", false,
                                      DataSourceUpdateMode.OnPropertyChanged);
            LinkPointEdit.DataBindings.Add("Text", _linkOperationData, "LinkTo", false,
                                           DataSourceUpdateMode.OnPropertyChanged);
            chkTargetFileOverwrite.DataBindings.Add("Checked", _linkOperationData, "OverwriteExistingFiles", false,
                                                    DataSourceUpdateMode.OnPropertyChanged);

            CopyToTarget.CheckedChanged +=
                (sender, e) => _linkOperationData.CopyBeforeDelete = CopyToTarget.Checked;
        }


        private void RegisterHandlers()
        {
            RegisterFolderBrowser(BrowsePoint, LinkPointEdit);
            RegisterFolderBrowser(BrowseTarget, LinkFrom);
            RegisterFileBrowser(BrowseFilePoint, LinkPointEdit);
            RegisterFileBrowser(BrowseFileTarget, LinkFrom);

            Go.Click += Go_Click;

            CopyToTarget.CheckedChanged +=
                (sender, e) => chkTargetFileOverwrite.Enabled = !chkTargetFileOverwrite.Enabled;
            CloseBtn.Click += (sender, e) => Application.Exit();
        }

        private void RegisterFileBrowser(Button browseFilePoint, TextBox linkPointEdit)
        {
            browseFilePoint.Tag = linkPointEdit;
            browseFilePoint.Click += BrowseForFile;
        }

        private void BrowseForFile(object sender, EventArgs e)
        {
            var button = sender as Button;
            var textBox = button.Tag as TextBox;
            if (textBox != null)
            {
                using (var browser = new OpenFileDialog())
                {
                    browser.CheckFileExists = false;
                    browser.DereferenceLinks = false;

                    if (textBox.Text.Trim() != String.Empty)
                    {
                        browser.InitialDirectory = textBox.Text;
                    }

                    if (browser.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = browser.FileName;
                    }
                }
            }
        }

        private void RegisterFolderBrowser(Button button, TextBox textbox)
        {
            button.Tag = textbox;
            button.Click += BrowseForFolder;
        }

        private void Go_Click(object sender, EventArgs e)
        {
            if (FormValid() && ValidOperation())
            {
                CallPerformOperation(new EventArgs());
            }
        }

        private bool FormValid()
        {
            Boolean valid = true;

            valid &= ValidateEditor(LinkPointEdit);
            valid &= ValidateEditor(LinkFrom);

            return valid;
        }

        private Boolean ValidateEditor(TextBox textBox)
        {
            var validEA = new ValidationArgs(textBox.Text);
            CallValidatePath(validEA);

            if (!validEA.Valid)
            {
                ErrorProvider.SetError(textBox, "Please enter a valid path");
                textBox.TextChanged += RealTimeValidation;
            }
            else
            {
                ErrorProvider.SetError(textBox, String.Empty);
                textBox.TextChanged -= RealTimeValidation;
            }

            return validEA.Valid;
        }

        private void RealTimeValidation(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            ValidateEditor(textBox);
        }

        private void BrowseForFolder(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is TextBox)
            {
                var textBox = button.Tag as TextBox;

                using (var browser = new FolderBrowserDialog())
                {
                    if (textBox.Text.Trim() != String.Empty)
                    {
                        browser.SelectedPath = textBox.Text;
                    }

                    if (browser.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = browser.SelectedPath;
                    }
                }
            }
            else
            {
                Trace.WriteLine("Delegate not registered to button or the textbox is specified.");
            }
        }
    }
}