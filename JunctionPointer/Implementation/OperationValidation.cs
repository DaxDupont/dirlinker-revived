using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Data;
using DirLinker.Interfaces;

namespace DirLinker.Implementation
{
    public class OperationValidation : IOperationValidation
    {
        private readonly IFileFactoryForPath _fileFactory;
        private IFolderFactoryForPath _folderFactory;

        public OperationValidation(IFileFactoryForPath fileFactory, IFolderFactoryForPath folderFactory)
        {
            _folderFactory = folderFactory;
            _fileFactory = fileFactory;
        }

        public Boolean ValidOperation(LinkOperationData linkData, out String errorMessage)
        {
            errorMessage = String.Empty;

            if(PathsMatch(linkData))
            {
                errorMessage = "A path can not be linked to itself";
                return false;
            }
            
            if(!IsValidFileLink(linkData))
            {
                errorMessage = "When creating a file link the linked to file must exist";
                return false;
            }

            if (TryingToLinkFileToFolder(linkData))
            {
                errorMessage = "A file can not be linked to a folder";
                return false;
            }

            return true;
        }

        private bool TryingToLinkFileToFolder(LinkOperationData linkData)
        {
            var linkToAsFolder = _folderFactory(linkData.LinkTo);
            var createLinkAtAsFile = _fileFactory(linkData.CreateLinkAt);
            
            if (linkToAsFolder.FolderExists() && createLinkAtAsFile.Exists())
            {
                return true;
            }

            return false;
        }

        private Boolean IsValidFileLink(LinkOperationData linkData)
        {
            IFile createLinkAtFile = _fileFactory(linkData.CreateLinkAt);
            IFile linkToFile = _fileFactory(linkData.LinkTo);

            if (linkToFile.Exists())
            {
                return createLinkAtFile.Exists() || linkData.CopyBeforeDelete;
            }

            return true;
        }

        private Boolean PathsMatch(LinkOperationData linkData)
        {
            return linkData.CreateLinkAt.Equals(linkData.LinkTo, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
