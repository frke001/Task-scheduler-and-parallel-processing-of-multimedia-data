using Raspoređivač_zadataka.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;
using System.Diagnostics;
using Raspoređivač_zadataka.Algorithms;
using System.Collections.ObjectModel;

namespace Raspoređivač_zadataka
{
    public class TaskScheduler
    {
        public readonly int maxNumberOfConcurentTasks;
        public readonly SchedulingAlgorithm schedulingAlgorithm;
        public ObservableCollection<Task> Tasks { get; } = new();
        //public readonly List<Task> runningTasks = new List<Task>();
        public readonly List<Task> scheduledWaitingTasks = new List<Task>();
        public readonly object schedulerLock = new();
        public Dictionary<Task, Stopwatch> taskExecutionTime = new Dictionary<Task, Stopwatch>(); // we keep a stopwatch for each task that measures the execution time
        public Dictionary<Task, ExecutionChecker> taskCheckerDictionary = new Dictionary<Task, ExecutionChecker>();
        private Dictionary<string,Resource> resources = new(); // all resources available to the system, identified by a unique string

        public TaskScheduler(int maxNumberOfThreads, SchedulingAlgorithm schedulingAlgorithm)
        {
            this.maxNumberOfConcurentTasks = maxNumberOfThreads;
            this.schedulingAlgorithm = schedulingAlgorithm;
        }

        public Task Schedule(TaskSpecification taskSpecification)
        {
            Task task = new Task(taskSpecification,
                HandleTaskTerminedted,
                HandleTaskPaused,
                HandleTaskContinueRequested,
                HandleTaskScheduleRequested,
                HandleTaskContextSwitch,
                HandleTryLock,
                HandleUnlock);            
            task.State = Task.TaskState.Scheduled;
            if (taskSpecification.ExucutionTime != null)
            {

                Stopwatch stopwatch = new Stopwatch();
                taskExecutionTime.Add(task, stopwatch);
                ExecutionChecker exCheck = new ExecutionChecker(task, stopwatch, taskSpecification.ExucutionTime.GetValueOrDefault());
                exCheck.Start();
            }
            if (taskSpecification.deadlineTime != null)
            {
                SetDeadLineTime(task, taskSpecification);
            }
            if (taskSpecification.startTime != null)
            {
                StartTaskAtSpecificTime(task, taskSpecification);
                return task;
            }
            {
                lock (schedulerLock)
                {
                    Tasks.Add(task);
                    if (schedulingAlgorithm.runningTasks.Count() < maxNumberOfConcurentTasks)
                    {

                        schedulingAlgorithm.BeforeExecution(task); // is added for execution
                        task.Start();
                        if (taskExecutionTime.ContainsKey(task))
                            taskExecutionTime[task].Start(); 
                    }
                    else
                    {
                        schedulingAlgorithm.AddTask(task);
                    }
                }
            }
            return task;
        }

        public Task ScheduleWithoutStart(TaskSpecification taskSpecification)
        {
            Task task = new Task(taskSpecification,
                HandleTaskTerminedted,
                HandleTaskPaused,
                HandleTaskContinueRequested,
                HandleTaskScheduleRequested,
                HandleTaskContextSwitch,
                HandleTryLock,
                HandleUnlock);            
            task.State = Task.TaskState.ScheduledWithoutStarting;
            if (taskSpecification.ExucutionTime != null)
            {

                Stopwatch stopwatch = new Stopwatch();
                taskExecutionTime.Add(task, stopwatch);
                ExecutionChecker exCheck = new ExecutionChecker(task, stopwatch, taskSpecification.ExucutionTime.GetValueOrDefault());
                taskCheckerDictionary.Add(task, exCheck);   
            }
            if (taskSpecification.deadlineTime != null)
            {
                SetDeadLineTime(task, taskSpecification);
            }
            if (taskSpecification.startTime != null)
            {
                StartTaskAtSpecificTime(task, taskSpecification);
            }
            lock (schedulerLock)
            {
                Tasks.Add(task);
                if (!scheduledWaitingTasks.Contains(task))
                    scheduledWaitingTasks.Add(task); // when a request arrives that they can be executed, they move to the running or waiting queue
            }

            return task;
        }
        // it has finished executing, it is removed from the running ones and the next one is taken from the queue according to some algorithm if the queue is not empty
        public void HandleTaskTerminedted(Task task)
        {
            TerminateOrWait(task);
        }
        public void HandleTaskPaused(Task task)
        {
            TerminateOrWait(task);
        }
        public void HandleTaskContinueRequested(Task task)
        {
            lock (schedulerLock)
            {
                if (schedulingAlgorithm.runningTasks.Count() < maxNumberOfConcurentTasks)
                {

                    schedulingAlgorithm.BeforeExecution(task);
                    task.Start();
                    if (taskExecutionTime.ContainsKey(task))
                        taskExecutionTime[task].Start();
                }
                else
                {
                    schedulingAlgorithm.AddTask(task); // waiting queue
                }
            }
        }
        public void HandleTaskScheduleRequested(Task task) // when it is deployed but waiting for the trigger and the trigger happens
        {
            lock (schedulerLock)
            {
                scheduledWaitingTasks.Remove(task);

                if (schedulingAlgorithm.runningTasks.Count() < maxNumberOfConcurentTasks)
                {
                    schedulingAlgorithm.BeforeExecution(task);
                    if (taskExecutionTime.ContainsKey(task))
                        taskExecutionTime[task].Start();
                    if(taskCheckerDictionary.ContainsKey(task) && task.State.Equals(Task.TaskState.Scheduled))
                        taskCheckerDictionary[task].Start();
                    task.Start();
                }
                else
                {
                    schedulingAlgorithm.AddTask(task);
                }
            }
        }
        private void StartTaskAtSpecificTime(Task task, TaskSpecification taskSpecification)
        {
            if (taskSpecification.startTime < DateTime.Now)
                throw new InvalidDateTimeException("Invalid task start time");
            task.State = Task.TaskState.ScheduledWithoutStarting;
            lock (schedulerLock)
            {
                scheduledWaitingTasks.Add(task);
                Timer timer = new Timer();
                timer.Interval = (taskSpecification.startTime.GetValueOrDefault() - DateTime.Now).TotalMilliseconds;
                timer.AutoReset = false;
                timer.Elapsed += (sender, e) =>
                {
                    if (task.State.Equals(Task.TaskState.ScheduledWithoutStarting))
                        task.StartScheduling();

                };
                timer.Start();
            }
        }
        private void SetDeadLineTime(Task task, TaskSpecification taskSpecification)
        {
            if (taskSpecification.deadlineTime < DateTime.Now)
                throw new InvalidDateTimeException("Invalid deadline time");
            Timer timer = new Timer();
            timer.Interval = (taskSpecification.deadlineTime.GetValueOrDefault() - DateTime.Now).TotalMilliseconds;
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) =>
            {
                if (!(task.State.Equals(Task.TaskState.Terminated)))
                    task.RequestTerminate(); // when the execution time expires

            };
            timer.Start();
        }
        private void HandleTaskContextSwitch(Task task)
        {
            TerminateOrWait(task);
        }
        private void TerminateOrWait(Task task)
        {

            lock (schedulerLock)
            {
                schedulingAlgorithm.AfterExecution(task); // kicking out of running
                if (taskExecutionTime.ContainsKey(task))
                    taskExecutionTime[task].Stop();
                Task nextTask = schedulingAlgorithm.RemoveTask();
                if (nextTask != null)
                {
                    schedulingAlgorithm.BeforeExecution(nextTask);
                    nextTask.Start();
                    if (taskExecutionTime.ContainsKey(nextTask))
                        taskExecutionTime[nextTask].Start();
                }
            }
        }
        private void HandleTryLock(string resourceName,Task task) 
        {
            bool status = resources.TryGetValue(resourceName, out Resource resource);
            if (status)
                resource.TryLock(task);
        }
        private void HandleUnlock(string resourceName)
        {
            bool status = resources.TryGetValue(resourceName, out Resource resource);
            if (status)
                resource.Unlock();
        }
        public void AddResource(Resource resource)
        {
            resources.Add(resource.ResourceName,resource);
        }
    }

}
