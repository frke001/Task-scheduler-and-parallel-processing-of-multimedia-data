using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    public interface IAction
    {
        public void Run(ICoOperative taskApi);
    }
}
