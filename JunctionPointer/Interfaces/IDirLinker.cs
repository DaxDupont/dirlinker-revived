using System;
using System.Windows.Forms;

namespace DirLinker.Interfaces
{

    
    public class FeedbackArgs
    {
        public String ProgressTitle { get; set; }
        public String Progress { get; set; }
    }

   // public delegate void UserMessage(Object sender, UserMessageArgs e);

    public delegate void ReportProgress(Object senser, FeedbackArgs e);

    public interface IDirLinker
    {
        //event UserMessage UserMessage;
        event ReportProgress ReportFeedback;

        bool ValidDirectoryPath(String path, out String ErrorMessage);
        bool CheckEnoughSpace(String target, String tempPath);
        void CreateSymbolicLinkFolder(String linkPoint, String linkTo, Boolean copyBeforeDelete, Boolean overwriteTargetFiles);
        void CopyFolder(IFolder source, IFolder target, Boolean overWriteTargetFile);
    }
}
