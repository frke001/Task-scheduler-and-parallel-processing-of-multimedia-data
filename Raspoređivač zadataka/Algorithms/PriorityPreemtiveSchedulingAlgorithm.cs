using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka.Algorithms
{

    public class PriorityPreemtiveSchedulingAlgorithm : PrioritySchedulingAlgorithm
    {

        override public void AddTask(Task task)
        {
            // if it was already arranged before, we just add it to the queue, and it will be taken when one of the tasks is completed
            /*if (!task.State.Equals(Task.TaskState.Scheduled))
            {
                base.AddTask(task);
                return;
            }*/
            Task swapTask = runningTasks.MinBy((t) => t.TaskSpecification.Priority); // we take the task with the lowest priority
            if (swapTask != null && swapTask.TaskSpecification.Priority < task.TaskSpecification.Priority) // if the incoming task is of higher priority than another, we throw out the task with the lowest priority
                swapTask.RequestContextSwitch();
            base.AddTask(task); // we always add that new one to the queue, because the condition for entering this function is full running tasks
            // he will be taken according to priority, certainly from the queue

        }
        override public Task RemoveTask()
        {
            return base.RemoveTask();
        }
        public override void AfterExecution(Task task)
        {
            base.AfterExecution(task);
            if (!task.State.Equals(Task.TaskState.Terminated) && !task.State.Equals(Task.TaskState.Paused) && !task.State.Equals(Task.TaskState.WaitingForResource))
                AddTask(task);
            // when a task is finished and if it is not completely finished or not paused it does not go to waitingArray, then
            // it is added to the queue again because it can be selected again, it is constantly competing for CPU
            // the situation when he is replaced by some task
        }

    }
}
