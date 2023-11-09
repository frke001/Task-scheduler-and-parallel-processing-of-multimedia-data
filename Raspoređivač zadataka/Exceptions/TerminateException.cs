using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka.Exceptions
{
    internal class TerminateException : Exception
    {

        public TerminateException() : base()
        {

        }
        public TerminateException(string message) : base(message)
        {
        }


    }
}
