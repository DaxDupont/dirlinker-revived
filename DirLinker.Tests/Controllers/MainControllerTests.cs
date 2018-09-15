using System;

using NUnit.Framework;
using DirLinker.Interfaces.Views;
using DirLinker.Interfaces;
using DirLinker.Controllers;
using Rhino.Mocks;
using DirLinker.Data;

namespace DirLinker.Tests.Controllers
{
    [TestFixture]
    public class MainControllerTests
    {

        [Test]
        public void Start_CreatesNewLinkData_SetsToView()
        {
            ILinkerView view = MockRepository.GenerateMock<ILinkerView>();
            MainController controller = new MainController(view, null, null, null, null);

            controller.Start();

            view.AssertWasCalled(v => v.SetOperationData(Arg<LinkOperationData>.Is.NotNull));
        }

        [Test]
        public void Start_RegisterValidatorPassedIn_ViewValidationDelegateIsRegistered()
        {
            ILinkerView view = MockRepository.GenerateMock<ILinkerView>();
            IPathValidation validator = MockRepository.GenerateMock<IPathValidation>();

            MainController controller = new MainController(view, validator, null, null, null);

            controller.Start();

            view.AssertWasCalled(v => v.ValidatePath += Arg<PathValidater>.Is.NotNull);
        }

        [Test]
        public void ValidatePath_ValidValidator_ValidatePathIsCalled()
        {

            ILinkerView view = MockRepository.GenerateMock<ILinkerView>();
            IPathValidation validator = MockRepository.GenerateMock<IPathValidation>();
            ValidationArgs args = new ValidationArgs("test");

            MainController controller = new MainController(view, validator, null, null, null);
            
            controller.ValidatePath(view, args);

            validator.AssertWasCalled(v => v.ValidPath(Arg<String>.Matches(s => s.Equals("test")), out Arg<String>.Out("").Dummy));   
        }

   


    }
}
