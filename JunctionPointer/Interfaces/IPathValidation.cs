using System;


namespace DirLinker.Interfaces
{
    public interface IPathValidation
    {
        Boolean ValidPath(String pathToValidate, out String errorMessage);
    }
}
