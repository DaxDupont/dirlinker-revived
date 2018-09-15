using System;
using System.Collections.Generic;


namespace DirLinker.Interfaces
{
    
    public interface ICommandDiscovery
    {
        List<ICommand> GetCommandListForFileTask(IFile linkTo, IFile linkFrom, bool copyBeforeDelete, bool overwriteTargetFiles);
        List<ICommand> GetCommandListTask(String linkTo, String linkFrom, bool copyBeforeDelete, bool overwriteTargetFiles);
        List<ICommand> GetCommandListForFolderTask(IFolder linkTo, IFolder linkFrom, Boolean copyBeforeDelete, Boolean overwriteTargetFiles);
    }
}
