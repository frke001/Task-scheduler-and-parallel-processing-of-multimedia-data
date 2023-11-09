using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka.Algorithms
{
    public abstract class SchedulingAlgorithm
    {
        public List<Task> runningTasks = new List<Task>();

        public abstract void AddTask(Task task);
        public abstract Task RemoveTask();

        public virtual void AfterExecution(Task task)
        {
            runningTasks.Remove(task);
        }
        public virtual void BeforeExecution(Task task)
        {
            runningTasks.Add(task);
        }
    }
}
