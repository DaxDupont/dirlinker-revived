using System;

namespace DirLinker.Interfaces
{
    public interface IOperatingSystemVersion
    {
        Boolean IsVistaOrLater();
        Boolean IsXp();
    }
}
