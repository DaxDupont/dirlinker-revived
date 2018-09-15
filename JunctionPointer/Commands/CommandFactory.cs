using System;
using DirLinker.Exceptions;
using DirLinker.Interfaces;

namespace DirLinker.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IOperatingSystemVersion _OsVersion;
        private readonly IJunctionPointXp _JunctionPointXp;

        public CommandFactory(IOperatingSystemVersion osVersion, IJunctionPointXp junctionPointXp)
        {
            _JunctionPointXp = junctionPointXp;
            _OsVersion = osVersion;   
        }

        public ICommand CreateFolderLinkCommand(IFolder linkTo, IFolder linkFrom)
        {
            if(_OsVersion.IsVistaOrLater())
            {
                return new CreateFolderLinkCommand(linkTo, linkFrom);
            }
            else
            {
                return new CreateLinkXpCommand(linkTo.FolderPath, linkFrom.FolderPath, _JunctionPointXp);
            }
        }

        public ICommand DeleteFolderCommand(IFolder folder)
        {
            return new DeleteFolderCommand(folder);
        }

        public ICommand CreateFolder(IFolder folder)
        {
            return new CreateFolderCommand(folder);
        }

        public ICommand MoveFileCommand(IFile source, IFile target, Boolean overwriteTarget)
        {
            return new MoveFileCommand(source, target, overwriteTarget);
        }


        public ICommand CreateFileLinkCommand(IFile linkTo, IFile linkFrom)
        {
            if (_OsVersion.IsVistaOrLater())
            {
                return new CreateFileLinkCommand(linkTo, linkFrom);
            }
            else
            {
               throw new DirLinkerException("File links are not supported on XP", DirLinkerStage.None);
            }
        }

        public ICommand DeleteFileCommand(IFile file)
        {
            return new DeleteFileCommand(file);
        }
     
    }
}
