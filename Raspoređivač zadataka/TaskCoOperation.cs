using Raspoređivač_zadataka.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    public class TaskCoOperation : ICoOperative
    {
        private Task task;

        public TaskCoOperation(Task task)
        {
            this.task = task;
        }
        public int GetMaxDegreeOfParallelism()
        {
            return task.TaskSpecification.MaxDegreeOfParallelism;
        }
        public void SetProgress(double progress)
        {
            this.task.Progress = progress;
        }
        public void CheckForPause()
        {
            lock (task.taskPauseLock)
            {
                if (task.State.Equals(Task.TaskState.RunningWithPauseRequest) || task.State.Equals(Task.TaskState.Paused))
                {
                    task.State = Task.TaskState.Paused;
                    task.onTaskPaused(task);
                    Monitor.Wait(task.taskPauseLock); // the calling thread is waiting to continue
                }
                else
                    return;
            }
        }
        public void CheckForTermination()
        {

            lock (task.taskLock)
            {
                if (task.State.Equals(Task.TaskState.RunningWithTerminateRequest))
                {                    
                    throw new TerminateException("Task terminated"); // we are interrupting the task
                }
                else
                    return;
            }
        }
        public void CheckForContextSwitch()
        {
            lock (task.contextSwitchLock)
            {
                if (task.State.Equals(Task.TaskState.RunningWithContexSwitchRequest) || task.State.Equals(Task.TaskState.WaitingToResumeContextSwitch))
                {
                    task.State = Task.TaskState.WaitingToResumeContextSwitch;
                    task.onTaskContextSwitch(task);
                    if(task.State.Equals(Task.TaskState.WaitingToResumeContextSwitch))
                        Monitor.Wait(task.contextSwitchLock); // the calling thread is waiting to continue
                }
                else
                    return;
            }
        }
        public void TryLock(string resourceName)
        {
            task.onTryLock(resourceName, task); // implemented in the scheduler
        }
        public void Unlock(string resourceName)
        {
            task.onUnlock(resourceName);
        }
    }
}
