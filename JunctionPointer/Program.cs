using System;
using System.Windows.Forms;
using DirLinker.Views;
using DirLinker.Controllers;
using DirLinker.Implementation;
using DirLinker.Interfaces;
using DirLinker.Interfaces.Controllers;
using DirLinker.Interfaces.Views;
using OCInject;
using DirLinker.Commands;

namespace DirLinker

{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ClassFactory classFactory = new ClassFactory();
            FillIoCContainer(classFactory);

            IMainController mainController = classFactory.ManufactureType<IMainController>();
            Application.Run(mainController.Start());
        }

        private static void FillIoCContainer(ClassFactory classFactory)
        {

            classFactory.RegisterType<IPathValidation, PathValidation>().AsSingleton();
            classFactory.RegisterType<IOperationValidation, OperationValidation>().AsSingleton();
            classFactory.RegisterType<IMainController, MainController>().AsSingleton();
            classFactory.RegisterType<ILinkerService, LinkerService>().AsSingleton();
            classFactory.RegisterType<ICommandDiscovery, CommandDiscovery>().AsSingleton();
            classFactory.RegisterType<ICommandFactory, CommandFactory>().AsSingleton();
            classFactory.RegisterType<IOperatingSystemVersion, OperatingSystemVersion>().AsSingleton();
            classFactory.RegisterType<IJunctionPointXp, JunctionPointXp>().AsSingleton();

            classFactory.RegisterType<ITransactionalCommandRunner, TransactionalCommandRunner>();
            classFactory.RegisterType<IWorkingView, ProgressView>();
            classFactory.RegisterType<ILocker, Locker>();
            classFactory.RegisterType<ILinkerView, DirLinkerView>();
            classFactory.RegisterType<IBackgroundWorker, BackgroundWorkerImp>();
            classFactory.RegisterType<ThreadSafeQueue<ICommand>>();
            
            classFactory.RegisterType<WorkerController>();

            classFactory.RegisterType<IMessenger, ThreadMessenger>()
                .WithFactory<ThreadMessengerFactory>();

            classFactory.RegisterType<IFolder, FolderImp>()
                .WithFactory<IFolderFactoryForPath>();

            classFactory.RegisterType<IFile, FileImp>()
                .WithFactory<IFileFactoryForPath>();
            
        }
    }
}
