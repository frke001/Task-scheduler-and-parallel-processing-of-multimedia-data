using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Raspoređivač_zadataka
{
    internal class Graph
    {
        private Dictionary<Task, Task> transitions = new Dictionary<Task, Task>();        


        public void TryAddTransition(Task currentTask,Task nextTask) // currentTask waits for nextTask, directed graph, the arrow points to the task from which we are waiting for the resource
        {
            transitions.Add(currentTask,nextTask);
            if (HasCyclus(currentTask))
            {
                transitions.Remove(currentTask);
                throw new ArgumentException("Deadlock!"); // deadlock prevention
            }

        }
        public void RemoveTransition(Task task)
        {
            transitions.Remove(task);
        }
        private bool HasCyclus(Task startTask)
        {
            HashSet<Task> visited = new HashSet<Task>();
            Stack<Task> cycleDetection = new Stack<Task>();
            return this.Dfs(visited, cycleDetection, startTask);
        }
        private bool Dfs(HashSet<Task> visited, Stack<Task> cycleDetection,Task startTask)
        {
            visited.Add(startTask);
            cycleDetection.Push(startTask);

            Task? nextTask = null;
            bool status = transitions.TryGetValue(startTask, out nextTask);
            if (status)
            {
                if (!visited.Contains(nextTask) && Dfs(visited, cycleDetection, nextTask))
                    return true;
                if (cycleDetection.Contains(nextTask))
                    return true;
            }
            cycleDetection.Pop();
            return false;
        }

    }
}
