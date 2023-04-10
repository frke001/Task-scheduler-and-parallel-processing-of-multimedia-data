using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{

    public interface ICoOperative
    {
        public int GetMaxDegreeOfParallelism();
        public void CheckForPause();
        public void CheckForTermination();
        public void CheckForContextSwitch();
        public void SetProgress(double progress);
        public void TryLock(string resourceName); // work with resources
        public void Unlock(string resourceName);
    }
}
