using System;
using System.Windows.Forms;

namespace DirLinker.Interfaces
{
    public delegate DialogResult RequestUserReponse(String question, MessageBoxButtons options);

    public interface ICommand
    {
        
        void Execute();
        void Undo();
        
        String UserFeedback { get; }

        event RequestUserReponse AskUser;
    }
}
