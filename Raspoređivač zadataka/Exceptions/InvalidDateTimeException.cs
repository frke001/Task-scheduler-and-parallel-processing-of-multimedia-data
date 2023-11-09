using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka.Exceptions
{
    internal class InvalidDateTimeException : Exception
    {
        public InvalidDateTimeException() : base()
        {

        }
        public InvalidDateTimeException(string message) : base(message)
        {
        }
    }
}
