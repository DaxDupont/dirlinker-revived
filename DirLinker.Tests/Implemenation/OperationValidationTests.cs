using System;
using DirLinker.Implementation;
using NUnit.Framework;
using DirLinker.Data;
using DirLinker.Interfaces;
using DirLinker.Tests.Helpers;
using DirLinker.Tests.Commands;

namespace DirLinker.Tests.Implemenation
{
    [TestFixture]
    public class OperationValidationTests
    {
        [Test]
        public void ValidOperation_ValidData_TrueIsReturned()
        {
            var validator = new OperationValidation(f => new FakeFile(f), f => new FakeFolder(f));

            var data = new LinkOperationData() { LinkTo = "test1", CreateLinkAt = "test2" };

            String errorMessage;
            Boolean valid = validator.ValidOperation(data, out errorMessage);

            Assert.IsTrue(valid);
        }

        [Test]
        public void ValidOperation_LinkAndPathDoNotMatch_FalseIsReturnedAlongWithErrorMessage()
        {
            var validator = new OperationValidation(f => new FakeFile(f), f => new FakeFolder(f));

            var data = new LinkOperationData() { LinkTo = "test", CreateLinkAt = "test" };
            String errorMessage;
            Boolean valid = validator.ValidOperation(data, out errorMessage);

            Assert.IsFalse(valid);
            Assert.AreEqual("A path can not be linked to itself", errorMessage);
        }

        [Test]
        public void ValidOperation__CreateLinkAtExistsNoCopyBeforeDeleteLinkToDoesNotExist_FalseIsReturnedAlongWithErrorMessage()
        {
            String createLinkAt = "linkAt";
            String linkTo = "linkTo";
            var fileFactory = GetFileFactoryThatReturnsExistsFor(createLinkAt);
            var validator = new OperationValidation(fileFactory, f => new FakeFolder(f));


            var data = new LinkOperationData() { LinkTo = createLinkAt, CreateLinkAt = linkTo, CopyBeforeDelete = false };

            String errorMessage;
            Boolean valid = validator.ValidOperation(data, out errorMessage);

            Assert.IsFalse(valid);
            Assert.AreEqual("When creating a file link the linked to file must exist", errorMessage);
        }

        [Test]
        public void ValidOperation_File_CreateLinkAtExistsCopyBeforeDeleteLinkToDoesNotExist_ValidOperation()
        {
            String createLinkAt = "linkAt";
            String linkTo = "linkTo";
            var fileFactory = GetFileFactoryThatReturnsExistsFor(createLinkAt);
            var validator = new OperationValidation(fileFactory, f => new FakeFolder(f));


            var data = new LinkOperationData() { LinkTo = createLinkAt, CreateLinkAt = linkTo, CopyBeforeDelete = true };

            String errorMessage;
            Boolean valid = validator.ValidOperation(data, out errorMessage);

            Assert.IsTrue(valid);
        }

        [Test]
        public void ValidOperation_File_LinkToExists_ValidOperation()
        {
            String createLinkAt = "linkAt";
            String linkTo = "linkTo";
            var fileFactory = GetFileFactoryThatReturnsExistsFor(linkTo);
            var validator = new OperationValidation(fileFactory, f => new FakeFolder(f));


            var data = new LinkOperationData() { LinkTo = createLinkAt, CreateLinkAt = linkTo, CopyBeforeDelete = true };

            String errorMessage;
            Boolean valid = validator.ValidOperation(data, out errorMessage);

            Assert.IsTrue(valid);
        }

        [Test]
        public void ValidOperation_AttemptsToLinkFileToFolder_Valid()
        {
            String createLinkAt = "linkAt";
            String linkTo = "linkTo";
            var fileFactory = GetFileFactoryThatReturnsExistsFor(createLinkAt);
            var folderFactory = CommandDiscoveryTests.GetFolderFactoryThatReturnsExistsFor(linkTo);
            var validator = new OperationValidation(fileFactory, folderFactory);


            var data = new LinkOperationData() { LinkTo = createLinkAt, CreateLinkAt = linkTo, CopyBeforeDelete = true };

            String errorMessage;
            Boolean valid = validator.ValidOperation(data, out errorMessage);

            Assert.IsTrue(valid);
        }

        [Test]
        public void ValidOperation_AttemptsToLinkFolderToFile_NotValid()
        {
            String createLinkAt = "linkAt";
            String linkTo = "linkTo";
            var fileFactory = GetFileFactoryThatReturnsExistsFor(createLinkAt);
            var folderFactory = CommandDiscoveryTests.GetFolderFactoryThatReturnsExistsFor(linkTo);
            var validator = new OperationValidation(fileFactory, folderFactory);


            var data = new LinkOperationData() { LinkTo = linkTo, CreateLinkAt = createLinkAt, CopyBeforeDelete = true };

            String errorMessage;
            Boolean valid = validator.ValidOperation(data, out errorMessage);

            Assert.IsFalse(valid);
        }

        private IFileFactoryForPath GetFileFactoryThatReturnsExistsFor(String fileToReturnTrueFor)
        {
            IFileFactoryForPath fileFactory = (f) =>
            {
                var fileToReturn = new FakeFile(f);

                fileToReturn.ExistsReturnValue = f.Equals(fileToReturnTrueFor);

                return fileToReturn;
            };

            return fileFactory;
        }

    }
}
