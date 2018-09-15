using System;
using System.Collections.Generic;
using DirLinker.Interfaces;
using System.IO;

namespace DirLinker.Commands
{
    public class CommandDiscovery : ICommandDiscovery
    {
        private readonly ICommandFactory _factory;
        private readonly IFileFactoryForPath _fileFactory;
        private readonly IFolderFactoryForPath _folderFactory;

        public CommandDiscovery(ICommandFactory factory, 
            IFileFactoryForPath fileFactory, 
            IFolderFactoryForPath folderFactory)
        {
            _factory = factory;
            _fileFactory = fileFactory;
            _folderFactory = folderFactory;
        }

        public List<ICommand> GetCommandListForFileTask(IFile linkTo, IFile linkFrom, bool copyBeforeDelete, bool overwriteTargetFiles)
        {
            var commandList = new List<ICommand>();

            if (linkTo.Exists())
            {
                if (copyBeforeDelete)
                {
                    var moveFileCommand = _factory.MoveFileCommand(linkTo, linkFrom, overwriteTargetFiles);
                    commandList.Add(moveFileCommand);
                }
                else
                {
                    var delFileCommand = _factory.DeleteFileCommand(linkTo);
                    commandList.Add(delFileCommand);
                }
            }

            commandList.Add(_factory.CreateFileLinkCommand(linkTo, linkFrom));

            return commandList;
        }

        public List<ICommand> GetCommandListTask(String linkTo, String linkFrom, bool copyBeforeDelete, bool overwriteTargetFiles)
        {
            if (IsFileLink(linkTo, linkFrom))
            {
                IFile targetFile = GetTargetFile(linkTo, linkFrom);
                return GetCommandListForFileTask(_fileFactory(linkTo), targetFile, copyBeforeDelete, overwriteTargetFiles);
            }
            else
            {
                if (ValidFolderLocation(linkTo))
                {
                    return GetCommandListForFolderTask(_folderFactory(linkTo), _folderFactory(linkFrom),
                                      copyBeforeDelete, overwriteTargetFiles);
                }
                else
                {
                    throw new InvalidOperationException("A file can not be linked to a folder");       
                }
            }
        }

        private bool ValidFolderLocation(String linkTo)
        {
            var file = _fileFactory(linkTo);
            return !file.Exists();
        }

        private IFile GetTargetFile(String linkTo, String linkFrom)
        {
            if (ValidFileLocation(linkTo))
            {
                var targetAsFile = _fileFactory(linkFrom);
                var targetAsFolder = _folderFactory(linkFrom);

                if (targetAsFolder.FolderExists() || linkFrom.EndsWith(@"\"))
                {
                    var finalFileName = Path.Combine(linkFrom, Path.GetFileName(linkTo));
                    return _fileFactory(finalFileName);
                }

                return targetAsFile;
            }
            else
            {
                throw new InvalidOperationException("A folder can not be linked to a file");
            }
        }

        private bool ValidFileLocation(String linkTo)
        {
            var folder = _folderFactory(linkTo);
            return !folder.FolderExists();
        }

        private bool IsFileLink(String linkTo, String linkFrom)
        {
            var linkToAsFile = _fileFactory(linkTo);
            var linkFromAsFile = _fileFactory(linkFrom);

            return linkToAsFile.Exists() || linkFromAsFile.Exists();
        }


        public List<ICommand> GetCommandListForFolderTask(IFolder linkTo, IFolder linkFrom, bool copyBeforeDelete, bool overwriteTargetFiles)
        {
           var commandList = new List<ICommand>();

           if (!linkFrom.FolderExists())
           {
               commandList.Add(_factory.CreateFolder(linkFrom));
           }

           if (linkTo.FolderExists())
           {
               if (copyBeforeDelete)
               {
                   commandList.AddRange(CreateFolderMoveOperations(linkTo, linkFrom, overwriteTargetFiles));
               }
               commandList.Add(_factory.DeleteFolderCommand(linkTo));
           }

           commandList.Add(_factory.CreateFolderLinkCommand(linkTo, linkFrom));
           
           return commandList;
        }

        private List<ICommand> CreateFolderMoveOperations(IFolder source, IFolder target, bool overwriteTargetFiles)
        {
            var moveFolderStructureCommands = new List<ICommand>();

            source.GetSubFolderList().ForEach(f =>
                {
                    String targetLocation = f.FolderPath.Replace(source.FolderPath, target.FolderPath);
                    IFolder moveTarget = _folderFactory(targetLocation);
                
                    if (!moveTarget.FolderExists())
                    {
                       moveFolderStructureCommands.Add(_factory.CreateFolder(moveTarget));
                    }

                    moveFolderStructureCommands.AddRange(CreateFolderMoveOperations(f, moveTarget, overwriteTargetFiles));
                });

            source.GetFileList().ForEach(f => 
                {
                    IFile targetFile = _fileFactory(Path.Combine(target.FolderPath, f.FileName));
                    
                    ICommand moveFileCommand = _factory.MoveFileCommand(f, targetFile, overwriteTargetFiles);
                    moveFolderStructureCommands.Add(moveFileCommand);
                });

            return moveFolderStructureCommands;
        }

    }
}
