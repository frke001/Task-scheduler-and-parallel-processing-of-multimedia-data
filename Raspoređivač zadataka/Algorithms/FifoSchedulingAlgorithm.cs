using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka.Algorithms
{
    public class FifoSchedulingAlgorithm : SchedulingAlgorithm
    {
        public Queue<Task> tasks = new Queue<Task>(); // a queue of pending tasks

        override public void AddTask(Task task)
        {
            tasks.Enqueue(task);
        }
        override public Task RemoveTask()
        {
            Task task = null;
            if (!isEmpty())
            {
                task = tasks.Dequeue();
            }
            return task;
        }
        public bool isEmpty()
        {
            if (tasks.Count == 0)
                return true;
            else return false;
        }
    }
}
