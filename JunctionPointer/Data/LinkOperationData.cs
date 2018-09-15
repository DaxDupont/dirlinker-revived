using System;
using System.ComponentModel;

namespace DirLinker.Data
{
    public class LinkOperationData : INotifyPropertyChanged
    {
        private String _CreateLinkAt;
        public String CreateLinkAt
        {
            get
            {
                return _CreateLinkAt;
            }
            set
            {
                _CreateLinkAt = value;
                NotifyPropertyChange("CreateLinkAt");
            }
        }

        private String _LinkTo;
        public String LinkTo
        {
            get
            {
                return _LinkTo;
            }
            set
            {
                _LinkTo = value;
                NotifyPropertyChange("LinkTo");
            }
        }

        private Boolean _CopyBeforeDelete;
        public Boolean CopyBeforeDelete
        {
            get
            {
                return _CopyBeforeDelete;
            }
            set
            {
                _CopyBeforeDelete = value;
                NotifyPropertyChange("CopyBeforeDelete");
            }
        }

        private Boolean _OverwriteExistingFiles;
        public Boolean OverwriteExistingFiles
        {
            get
            {
                return _OverwriteExistingFiles;
            }
            set
            {
                _OverwriteExistingFiles = value;
                NotifyPropertyChange("OverwriteExistingFiles");
            }
        }



        private PropertyChangedEventHandler _PropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _PropertyChanged += value; }
            remove { _PropertyChanged -= value; }
        }

        private void NotifyPropertyChange(String propName)
        {
            PropertyChangedEventHandler callee = _PropertyChanged;
            if (callee != null)
            {
                callee(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}
