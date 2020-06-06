using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    public class WrongMatchPhase : Exception
    {
        public WrongMatchPhase() : base() { }
        public WrongMatchPhase(string message) : base(message) { }
        public WrongMatchPhase(string message, Exception innerException) : base(message, innerException) { }
    }
}
