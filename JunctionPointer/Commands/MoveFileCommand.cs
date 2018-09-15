using System;
using DirLinker.Interfaces;
using DirLinker.Exceptions;
using System.IO;
using System.Windows.Forms;

namespace DirLinker.Commands
{
    public delegate ICommand CopyFileCommandFactory(IFile folder);

    public class MoveFileCommand : ICommand
    {
        private IFile _Source;
        private IFile _Target;
        private Boolean _Overwrite;
        private Boolean _FileMoved;

        public MoveFileCommand(IFile source, IFile target, Boolean overwrite)
        {
            _Source = source;
            _Target = target;
            _Overwrite = overwrite;
            _FileMoved = false;
        }

        public void Execute()
        {
            Boolean canCopyToTarget = TargetWriteable();

            if (canCopyToTarget)
            {
                _Source.MoveFile(_Target);
                _FileMoved = true;
            }

        }

        private Boolean TargetWriteable()
        {
            Boolean targetWriteable = true;

            if (_Target.Exists() && _Overwrite)
            {
                targetWriteable = !TargetFileReadOnly();
                if(targetWriteable)
                {
                    _Target.Delete();
                }
            }
            else if(_Target.Exists() && !_Overwrite)
            {
                targetWriteable = false;
            }

            return targetWriteable;
        }

        private Boolean TargetFileReadOnly()
        {
            Boolean targetFileReadonly = false;
            
            if ((_Target.GetAttributes() & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                DialogResult res = RequestUserRespone(String.Format("{0} is read only.  Would you like to overwrite it?", _Target.FullFilePath));
           
                if (res == DialogResult.Yes)
                {
                    _Target.SetAttributes(FileAttributes.Normal);
                }
                else if (res == DialogResult.No)
                {
                    targetFileReadonly = true;
                }
                else if (res == DialogResult.Cancel)
                {
                    throw new DirLinkerException("User requested cancel", DirLinkerStage.CopyingSourceToTemp);
                }
            }

            return targetFileReadonly;
        }

        private DialogResult RequestUserRespone(String message)
        {
            RequestUserReponse ask = _AskUser;
            DialogResult res = DialogResult.Cancel;

            if (ask != null)
            {
                res = _AskUser(message, MessageBoxButtons.YesNoCancel);
            }

            return res;
        }

        public void Undo()
        {
            if (_FileMoved && !_Source.Exists())
            {
                _Target.MoveFile(_Source);
                _Target.Delete();
            }
        }

        public String UserFeedback
        {
            get
            {
                return String.Format("Copying file: {0} to {1}", _Source.FullFilePath, _Target.FullFilePath);
            }
        }
        
        protected event RequestUserReponse _AskUser;
        
        public event RequestUserReponse AskUser
        {
            add { _AskUser += value; }
            remove { _AskUser -= value; }
        }
    }
}
