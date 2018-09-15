using System;
using System.Linq;
using DirLinker.Interfaces;
using System.Text.RegularExpressions;

namespace DirLinker.Implementation
{
    public class PathValidation : IPathValidation
    {
        private readonly IFolderFactoryForPath _folderFactory;
        private readonly IOperatingSystemVersion _operatingSystemVersion;
        private const String RegexForDrive = @"^([a-zA-Z]\:)";
        private readonly String regexForUncPAth = @"^(\\{2})([^/:*?<>""|]*)+";

        public PathValidation(IFolderFactoryForPath folderFactory, IOperatingSystemVersion operatingSystemVersion)
        {
            _folderFactory = folderFactory;
            _operatingSystemVersion = operatingSystemVersion;
        }


        public Boolean ValidPath(String path, out String errorMessage)
        {
            errorMessage = String.Empty;
            IFolder folder = _folderFactory(String.Empty);

            if (!String.IsNullOrEmpty(path))
            {
                if (IsDriveLetter(path))
                {
                    errorMessage = "Only file or folder paths allowed";
                    return false;
                }

                if (path.Length > folder.MaxPath())
                {
                    errorMessage = "Selected path is longer than the maximum allowable Windows path";
                    return false;
                }

                String preappend = String.Empty;

                if (path.StartsWith(@"\\"))
                {
                    preappend = @"\\";
                    path = path.Remove(0, 2);
                }

                String[] pathParts = path.Split('\\');
                if (pathParts.Length == 0)
                {
                    errorMessage = "The path is not well formed";
                    return false;
                }
                
                if(IsValidLocation(preappend + pathParts[0]))
                {
                    if (pathParts.Length > 1)
                    {
                        String pathWithoutDrive = path.Replace(pathParts[0], String.Empty);

                        Int32 count = folder.GetIllegalPathChars().Count(c => pathWithoutDrive.Contains(c));

                        if (count > 0)
                        {
                            errorMessage = "Path contains illegal characters";
                            return false;
                        }
                    }
                }
                else
                {
                    errorMessage = "Invalid path";
                    return false;
                }

            }
            else
            {
                errorMessage = "Please enter a path";
                return false;
            }

            return true;
        }

        private Boolean IsValidLocation(String pathPart)
        {
            if(!Regex.IsMatch(pathPart, RegexForDrive))
            {
                return !_operatingSystemVersion.IsXp() && Regex.IsMatch(pathPart, regexForUncPAth);
            }
            return true;
        }

        private bool IsDriveLetter(String path)
        {
            if (path.Length < 4 && Regex.IsMatch(path, RegexForDrive))
            {
                return true;
            }

            return false;
        }
        
    }
}
