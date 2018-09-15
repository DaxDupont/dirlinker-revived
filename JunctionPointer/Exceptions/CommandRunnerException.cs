using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DirLinker.Exceptions
{
    public class CommandRunnerException : Exception
    {
        public CommandRunnerException(string message)
            : base(message)
        {
            
        }

        public CommandRunnerException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
