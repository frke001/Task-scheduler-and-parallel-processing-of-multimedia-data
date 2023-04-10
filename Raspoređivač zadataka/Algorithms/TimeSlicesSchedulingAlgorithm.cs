using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Raspoređivač_zadataka.Algorithms
{
    public class TimeSlicesSchedulingAlgorithm : FifoSchedulingAlgorithm
    {
        public int timeSlice = 1000;

        public TimeSlicesSchedulingAlgorithm()
        {

        }
        public TimeSlicesSchedulingAlgorithm(int timeSlice) : base()
        {
            this.timeSlice = timeSlice;
        }

        public override void AfterExecution(Task task)
        {
            base.AfterExecution(task);
            if (!task.State.Equals(Task.TaskState.Terminated) && !task.State.Equals(Task.TaskState.Paused))
                AddTask(task);
        }
        public override void BeforeExecution(Task task)
        {
            int prior = task.TaskSpecification.Priority;
            if (task.TaskSpecification.Priority == 0)
                prior = task.TaskSpecification.Priority + 1;
            base.BeforeExecution(task);
            Timer timer = new Timer();
            timer.Interval = this.timeSlice * prior; // depending on the priority, the period that the task occupies the CPU also depends
            // every time before starting we set the interval (timeSlice) when the context switch will happen
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) =>
            {
                if(task.State.Equals(Task.TaskState.Running)) // on the beat of the clock, the task changes, another takes over the processor
                    task.RequestContextSwitch();

            };
            timer.Start();
        }
    }
}
