using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka.Exceptions
{
    internal class InvalidTaskStateException : Exception
    {
        public InvalidTaskStateException() : base()
        {

        }
        public InvalidTaskStateException(string message) : base(message)
        {
        }

    }
}
