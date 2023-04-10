using Raspoređivač_zadataka.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    public class Task : INotifyPropertyChanged
    {
        public enum TaskState
        {
            NotScheduled,
            Scheduled,
            ScheduledWithoutStarting,
            Running,
            RunningWithPauseRequest,
            WaitingToResume,
            Paused,
            RunningWithContexSwitchRequest,
            WaitingToResumeContextSwitch,
            WaitingForResource,
            WaitingForResourceResume,
            RunningWithTerminateRequest,
            Terminated
        }
        private string _name;
        public TaskSpecification? TaskSpecification { get; set; }
        private TaskState _state;
        private double progress;
        private Thread? taskThread;
        public object taskLock = new();
        public object taskPauseLock = new();
        public object checkLock = new();
        public object contextSwitchLock = new();
        public object resourceLock = new();
        public Action<Task> onTaskTerminated;
        public Action<Task> onTaskPaused;
        public Action<Task> onTaskContinueRequested;
        public Action<Task> onTaskScheldueRequested;
        public Action<Task> onTaskContextSwitch;
        public Action<string,Task> onTryLock;
        public Action<string> onUnlock; // callback mechanism

        public event PropertyChangedEventHandler? PropertyChanged;

        public Task()
        {

        }
        //Action<Task> onJobPaused,Action<Task> onJobContinueRequested, Action<Task> onJobTerminateRequested
        public Task(TaskSpecification? TaskSpecification, Action<Task> onTaskTerminated,
            Action<Task> onTaskPaused, Action<Task> onTaskContinueRequested, Action<Task> onTaskScheldueRequested, Action<Task> onTaskContextSwitch,
            Action<string,Task> onTryLock, Action<string> onUnlock)
        {
            taskThread = new(() =>
            {
                try
                {
                    TaskSpecification.Action.Run(new TaskCoOperation(this));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Finish();
                }
            });
            taskThread.IsBackground = true;           
            this.State = Task.TaskState.NotScheduled;
            this.TaskSpecification = TaskSpecification;
            this.Name = this.TaskSpecification.Name;
            this.onTaskTerminated = onTaskTerminated;
            this.onTaskPaused = onTaskPaused;
            this.onTaskContinueRequested = onTaskContinueRequested;
            this.onTaskScheldueRequested = onTaskScheldueRequested;
            this.onTaskContextSwitch = onTaskContextSwitch;
            this.onUnlock = onUnlock;
            this.onTryLock = onTryLock;
        }


        public void Start()
        {
            try
            {
                lock (taskLock)
                {
                    if (State.Equals(TaskState.Scheduled))
                    {
                        State = TaskState.Running;
                        if (taskThread != null)
                            taskThread.Start();

                    }
                    else if (State.Equals(TaskState.Running) || State.Equals(TaskState.Terminated))
                        throw new InvalidTaskStateException("Task already started!");

                }
                lock (taskPauseLock)
                {
                    if (State.Equals(TaskState.WaitingToResume))
                    {
                        State = TaskState.Running;
                        Monitor.PulseAll(taskPauseLock);
                    }
                }
                lock (contextSwitchLock)
                {
                    if (State.Equals(TaskState.WaitingToResumeContextSwitch))
                    {
                        State = TaskState.Running;
                        Monitor.Pulse(contextSwitchLock);
                    }
                }
                lock (resourceLock)
                {
                    if (State.Equals(TaskState.WaitingForResourceResume))
                    {
                        State = TaskState.Running;
                        Monitor.Pulse(resourceLock);
                    }
                }
            }
            catch (InvalidTaskStateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void StartScheduling()
        {
            try
            {
                lock (taskLock)
                {
                    if (State.Equals(TaskState.ScheduledWithoutStarting)) // task added but waiting for trigger
                    {
                        State = TaskState.Scheduled; // this state is set before start
                        onTaskScheldueRequested(this); // callback
                    }
                    else
                        throw new InvalidTaskStateException("Inalid task state!");
                }
            }
            catch (InvalidTaskStateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Finish()
        {
            try
            {
                if (State.Equals(TaskState.NotScheduled) || State.Equals(TaskState.Scheduled) || State.Equals(TaskState.ScheduledWithoutStarting))
                    throw new InvalidTaskStateException("Task has not started yet!");
                
                else if (State.Equals(TaskState.Terminated))
                    return;
                else
                {
                    State = TaskState.Terminated;
                    NotifyAll();
                    onTaskTerminated(this);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Wait()
        {
            lock (taskLock)
            {
                if (State.Equals(TaskState.Terminated))
                    return;
                else
                {
                    Monitor.Wait(taskLock);
                }

            }
        }
        public void NotifyAll()
        {
            lock (taskLock)
            {
                Monitor.PulseAll(taskLock);
            }
        }
        public void RequestPause()
        {
            Console.WriteLine("pause requested");
            lock (taskLock)
            {
                if (State.Equals(TaskState.Running))
                {
                    State = TaskState.RunningWithPauseRequest;
                }
            }
        }
        public void RequestContinue()
        {
            Console.WriteLine("continue requested");
            lock (taskPauseLock)
            {
                if (State.Equals(TaskState.RunningWithPauseRequest)) // pause requested but not yet stopped
                {
                    State = TaskState.Running; // as if nothing had happened
                }
                else if (State.Equals(TaskState.Paused))
                {
                    State = TaskState.WaitingToResume;
                    onTaskContinueRequested(this); // callback to perform its redeployment
                }
            }
        }
        public void RequestTerminate()
        {
            Console.WriteLine("terminate requested");
            try
            {
                lock (taskLock)
                {

                    if (State.Equals(TaskState.Scheduled) || State.Equals(TaskState.ScheduledWithoutStarting)
                        || State.Equals(TaskState.NotScheduled))
                        throw new InvalidTaskStateException("Task is not running");
                    else if (State.Equals(TaskState.Terminated))
                        throw new InvalidTaskStateException("Task already terminated");
                    else
                    {
                        State = TaskState.RunningWithTerminateRequest;
                    }
                }
            }
            catch (InvalidTaskStateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void RequestContextSwitch()
        {
            Console.WriteLine("context switch requested");
            lock (taskLock)
            {
                if (State.Equals(TaskState.Running))
                {
                    State = TaskState.RunningWithContexSwitchRequest;
                }
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public TaskState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();   // without parameters takes the name from the calling method           
                OnPropertyChanged(nameof(IsStartable));
                OnPropertyChanged(nameof(IsPausable));
                OnPropertyChanged(nameof(IsStoppable));
                OnPropertyChanged(nameof(IsCloseable));
            }
        }
        [JsonIgnore]
        public bool IsStartable
        {
            get
            {
                lock (taskLock)
                {
                    return (State == Task.TaskState.Paused || State == Task.TaskState.ScheduledWithoutStarting
                        || State == Task.TaskState.RunningWithPauseRequest);
                }
            }
        }
        [JsonIgnore]
        public bool IsPausable
        {
            get
            {
                lock (taskLock)
                {
                    return (State == Task.TaskState.Running);
                }
            }
        }
        [JsonIgnore]
        public bool IsStoppable
        {
            get
            {
                lock (taskLock)
                {

                    return (State != Task.TaskState.Scheduled && State != Task.TaskState.NotScheduled 
                        && State != Task.TaskState.ScheduledWithoutStarting && State != Task.TaskState.Terminated 
                        && State != Task.TaskState.Paused);                    
                }
            }
        }
        [JsonIgnore]
        public bool IsCloseable
        {
            get
            {
                lock (taskLock)
                {
                    return (State == Task.TaskState.Paused || State == Task.TaskState.Terminated);                    
                }
            }
        }
        public double Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                OnPropertyChanged();
            }
        }
        
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            // if a change happens it will notify the GUI thread
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void BlockForResourse()
        {
            lock (taskLock)
            {
                State = Task.TaskState.WaitingForResource;
            }
            onTaskPaused(this); // it is removed from running and the next task is taken from the queue
            lock (resourceLock)
            {
                Monitor.Wait(resourceLock);
            }
        }
        public void UnblockForResource()
        {
            lock (taskLock)
            {
                if (State.Equals(Task.TaskState.WaitingForResource))
                {
                    State = Task.TaskState.WaitingForResourceResume; // waiting for start
                    onTaskContinueRequested(this);
                }
            }
        }
    }
}
