using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;

namespace DirLinker.Commands
{
    public interface ICommandFactory
    {
        ICommand MoveFileCommand(IFile source, IFile target, Boolean overwriteTarget);
        ICommand CreateFolder(IFolder folder);
        ICommand DeleteFolderCommand(IFolder folder);
        ICommand DeleteFileCommand(IFile file);
        ICommand CreateFolderLinkCommand(IFolder linkTo, IFolder linkFrom);
        ICommand CreateFileLinkCommand(IFile linkTo, IFile linkFrom);
    }
}
