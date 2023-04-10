using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    public class ExecutionChecker
    {
        private Task task;
        private Stopwatch stopwatch;
        private long executionTime;

        public ExecutionChecker(Task task, Stopwatch stopwatch, long executionTime)
        {
            this.task = task;
            this.stopwatch = stopwatch;
            this.executionTime = executionTime;
        }
        public void Start()
        {
            new Thread(Run).Start();
        }
        private void Run()
        {
            while (true)
            {
                bool status = false;
                Thread.Sleep(100);
                lock (task.taskLock)
                {
                    if (task.State == Task.TaskState.Terminated)
                        break;
                    if (task.State == Task.TaskState.Paused)
                    {
                        status = true;
                    }
                }
                if (status)
                {
                    lock (task.taskPauseLock)
                    {
                        Monitor.Wait(task.taskPauseLock);
                    }
                }
                if (stopwatch.ElapsedMilliseconds >= executionTime) // if the expected execution time passes
                {
                    task.RequestTerminate();
                    break;
                }
            }
        }
    }
}
