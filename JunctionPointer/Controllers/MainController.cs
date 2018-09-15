using System;
using DirLinker.Interfaces.Views;
using System.Windows.Forms;
using DirLinker.Interfaces.Controllers;
using DirLinker.Interfaces;
using DirLinker.Data;

namespace DirLinker.Controllers
{
    public class MainController : IMainController
    {
        private readonly ILinkerService _linkerService;
        private readonly IPathValidation _pathValidator;
        private readonly ILinkerView _view;
        private Func<WorkerController> _workerFactory;
        private readonly IOperationValidation _operationValidation;

        private LinkOperationData _operationData;
        
        public MainController(ILinkerView view, IPathValidation pathValidator, ILinkerService linkerService, Func<WorkerController> workerFactory, IOperationValidation operationValidation)
        {
            _view = view;
            _pathValidator = pathValidator;
            _linkerService = linkerService;
            _workerFactory = workerFactory;
            _operationValidation = operationValidation;
        }

        public Form Start()
        {
            _operationData = new LinkOperationData();
            
            _view.SetOperationData(_operationData);
            _view.ValidatePath += ValidatePath;
            _view.PerformOperation += PerformOperation;
            _view.ValidOperation = ValidateOperationData;
            return _view.MainForm;
        }

        public void ValidatePath(object sender, ValidationArgs e)
        {
            String errorMessage;
            if (_pathValidator.ValidPath(e.PathToValidate, out errorMessage))
            {
                e.Valid = true;
            }
            else
            {
                e.Valid = false;
                e.ErrorMessge = errorMessage;
            }
        }

        public void PerformOperation(object sender, EventArgs e)
        {

            _linkerService.SetOperationData(_operationData);
            var worker = _workerFactory();
            worker.ShowWorker(_view.MainForm);
        }

        private Boolean ValidateOperationData()
        {
            String message;

            if (!_operationValidation.ValidOperation(_operationData, out message))
            {
                _view.ShowMesage(message);
                return false;
            }

            return true;
        }
    }
}
