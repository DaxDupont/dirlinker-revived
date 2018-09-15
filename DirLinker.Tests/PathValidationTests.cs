using System;
using NUnit.Framework;
using DirLinker.Interfaces;
using DirLinker.Implementation;
using Rhino.Mocks;

namespace DirLinker.Tests
{
    [TestFixture]
    public class PathValidationTests
    {

        private readonly String m_Path = @"C:\Test\Path";

        private String GetPathAddingChar(Char c)
        {
            return String.Concat(m_Path, c);
        }

        [Test]
        public void ValidatePath_ValidPath_Valid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(m_Path, out errorMessage);

            //Assert
            Assert.IsTrue(valid);
            Assert.IsEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_EmptyString_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

        
            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(String.Empty, out errorMessage);


            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_SpaceOnly_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath("  ", out errorMessage);

            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_GreaterThanMaxPath_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIDirectoryManagerStub(1);

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            String longPath = @"c:\testPathThatIsLongerTheDirManagerAllows";


            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(longPath, out errorMessage);


            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_PathContainsIllegalCharacter_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIDirectoryManagerStub(new Char[] { '|', '?', ':' });

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(GetPathAddingChar('|'), out errorMessage);


            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_DriveHasColonPathDoesnContainAnyIllegalCharacters_Valid()
        {
            //MSDN states that : is illegal and is returned so need to check that when a drive is there it is still valid
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIDirectoryManagerStub(new Char[] { '|', '?', ':' });

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(m_Path, out errorMessage);


            //Assert
            Assert.IsTrue(valid);
            Assert.IsEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_DriveOnly_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(@"C:\", out errorMessage);


            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_NoDriveAtStart_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(@"thisIsNotAPath", out errorMessage);


            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_NoDriveAtStartOneBackSlash_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(@"\test", out errorMessage);


            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_3CharAtStartInvalidDrive_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion());

            //Act
            String errorMessage;
            Boolean valid = controller.ValidPath(@"abc\test", out errorMessage);

            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void ValidatePath_UNCPathOnXp_Invalid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion(isVista:false));

            //Act
            String errorMsg;
            Boolean valid = controller.ValidPath(@"\\network\share", out errorMsg);

            //Assert
            Assert.IsFalse(valid);
            Assert.IsNotEmpty(errorMsg);
        }

        [Test]
        public void ValidatePath_UNCPathOnVista_Valid()
        {
            //Arrange
            IFolder dirManager = Helpers.CreateStubHelpers.GetIFolderStub();

            IPathValidation controller = new PathValidation(s => dirManager, GetMockOperatingSystemVersion(isXp:false));

            //Act
            String errorMsg;
            Boolean valid = controller.ValidPath(@"\\network\share", out errorMsg);

            //Assert
            Assert.IsTrue(valid);
            Assert.IsEmpty(errorMsg);
        }

        protected IOperatingSystemVersion GetMockOperatingSystemVersion(Boolean isXp = true, Boolean isVista = true)
        {
            var osVersion = MockRepository.GenerateStub<IOperatingSystemVersion>();
            osVersion.Stub(o => o.IsVistaOrLater()).Return(isVista);
            osVersion.Stub(o => o.IsXp()).Return(isXp);
            return osVersion;
        }
    }
}
