using Raspoređivač_zadataka;
using Raspoređivač_zadataka.Algorithms;
using TaskScheduler = Raspoređivač_zadataka.TaskScheduler;
using Task = Raspoređivač_zadataka.Task;
using TaskSpecification = Raspoređivač_zadataka.TaskSpecification;
using System.Threading.Tasks;

namespace SchedulerTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ScheduleTaskTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task = taskScheduler.Schedule(new TaskSpecification(new DemoTask1()));
            lock (task)
            {
                Assert.AreEqual(Task.TaskState.Running, task.State);
            }
            
        }
        [Test]
        public void ScheduleTaskWithoutStarting()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task = taskScheduler.ScheduleWithoutStart(new TaskSpecification(new DemoTask1()));
            lock (task)
            {
                Assert.AreEqual(Task.TaskState.ScheduledWithoutStarting, task.State);
            }
            task.StartScheduling();
            lock (task)
            {
                Assert.AreEqual(Task.TaskState.Running, task.State);
            }
        }
        [Test]
        public void ScheduleWhileOtherTasksAreExecuting()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task1 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1()));
            Task task2 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1()));
            lock (task2)
            {
                Assert.AreEqual(Task.TaskState.Scheduled, task2.State);
            }
            task1.Wait();
            Thread.Sleep(500);
            lock (task2)
            {
                Assert.AreEqual(Task.TaskState.Running, task2.State);
            }
        }
        [Test]
        public void PauseTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task1 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1()));
            task1.RequestPause();
            Thread.Sleep(500);
            lock (task1)
            {
                Assert.AreEqual(Task.TaskState.Paused, task1.State);
            }
        }
        [Test]
        public void TerminateTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task1 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1()));
            task1.RequestTerminate();
            Thread.Sleep(1000);
            lock (task1)
            {
                Assert.AreEqual(Task.TaskState.Terminated, task1.State);
            }
        }
        [Test]
        public void ContinueTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task1 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1()));
            task1.RequestPause();
            Thread.Sleep(500);
            task1.RequestContinue();
            lock (task1)
            {
                Assert.AreEqual(Task.TaskState.Running, task1.State);
            }
        }
        [Test]
        public void WaitingForTaskToEndTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task1 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1()));
            task1.Wait();
            Console.WriteLine("After wait");
            lock (task1)
            {
                Assert.AreEqual(Task.TaskState.Terminated, task1.State);
            }
        }
        [Test]
        public void PriorityPreemptiveSchedulingTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new PriorityPreemtiveSchedulingAlgorithm());
            Task task1 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1(), priority: 2));
            Thread.Sleep(500);
            lock (task1)
            {
                Assert.AreEqual(Task.TaskState.Running, task1.State);
            }
            Task task2 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1(), priority:5));
            Thread.Sleep(500);
            lock (task2)
            {
                Assert.AreEqual(Task.TaskState.Running, task2.State);
            }
        }
        [Test]
        public void StartDateTimeTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new PriorityPreemtiveSchedulingAlgorithm());
            Task task = taskScheduler.Schedule(new TaskSpecification(new DemoTask1(), startTime: DateTime.Now.AddSeconds(2)));
            lock (task)
            {
                Assert.AreEqual(Task.TaskState.ScheduledWithoutStarting, task.State);
            }
            Thread.Sleep(3000);
            lock (task)
            {
                Assert.AreEqual(Task.TaskState.Running, task.State);
            }
        }
        [Test]
        public void DeadlineDateTimeTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task = taskScheduler.Schedule(new TaskSpecification(new DemoTask1(), deadlineTime: DateTime.Now.AddSeconds(2)));
            Thread.Sleep(4000);
            lock (task)
            {
                Assert.AreEqual(Task.TaskState.Terminated, task.State);
            }
        }
        [Test]
        public void ExecutionTimeTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(1, new FifoSchedulingAlgorithm());
            Task task = taskScheduler.Schedule(new TaskSpecification(new DemoTask1(), exucutionTime: 2000));
            Thread.Sleep(4000);
            lock (task)
            {
                Assert.AreEqual(Task.TaskState.Terminated, task.State);
            }
        }
        [Test]
        public void ResourcesTest()
        {
            TaskScheduler taskScheduler = new TaskScheduler(2, new FifoSchedulingAlgorithm());
            taskScheduler.AddResource(new Resource("res1"));
            Task task1 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1(),priority: 3));
            Task task2 = taskScheduler.Schedule(new TaskSpecification(new DemoTask1(),priority: 5));
            Thread.Sleep(500);
            lock (task1)
            {
                Assert.AreEqual(Task.TaskState.Running, task1.State);
            }
            lock (task2)
            {
                Assert.AreEqual(Task.TaskState.WaitingForResource, task2.State);
            }
            
        }

    }
}