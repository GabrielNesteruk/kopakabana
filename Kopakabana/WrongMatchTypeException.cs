using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    public class WrongMatchTypeException : Exception
    {
        public WrongMatchTypeException() : base() { }
        public WrongMatchTypeException(string message) : base(message) { }
        public WrongMatchTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
