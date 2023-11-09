using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka.Algorithms
{
    public class PrioritySchedulingAlgorithm : SchedulingAlgorithm
    {
        public PriorityQueue<Task, int> tasks = new PriorityQueue<Task, int>(Comparer<int>.Create((x, y) => y - x));

        override public void AddTask(Task task)
        {
            tasks.Enqueue(task, task.TaskSpecification.Priority);
        }
        override public Task RemoveTask()
        {
            Task task = null;
            if (IsEmpty())
                return null;
            task = tasks.Dequeue();
            return task;

        }
        public bool IsEmpty()
        {
            if (tasks.Count == 0)
                return true;
            else return false;
        }
    }
}
