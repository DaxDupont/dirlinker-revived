using System;
using DirLinker.Data;

namespace DirLinker.Interfaces
{
    public interface IOperationValidation
    {
        Boolean ValidOperation(LinkOperationData linkData, out String errorMessage);
    }
}