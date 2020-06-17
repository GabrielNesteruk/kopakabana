using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    public class WrongMatchPhaseException : Exception
    {
        public WrongMatchPhaseException() : base() { }
        public WrongMatchPhaseException(string message) : base(message) { }
        public WrongMatchPhaseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
