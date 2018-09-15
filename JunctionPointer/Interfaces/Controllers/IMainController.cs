using System;
using DirLinker.Interfaces.Views;
using DirLinker.Interfaces;
using System.Windows.Forms;

namespace DirLinker.Interfaces.Controllers
{
    public interface IMainController
    {
        void ValidatePath(object sender, ValidationArgs e);
        void PerformOperation(object sender, EventArgs e);

        Form Start();

    }
}
