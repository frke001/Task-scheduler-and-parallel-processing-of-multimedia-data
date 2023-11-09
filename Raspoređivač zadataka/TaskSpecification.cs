using System.Text.Json.Serialization;

namespace Raspoređivač_zadataka
{
    public class TaskSpecification
    {
        /*internal enum TaskPriority
        {
            Low = 1,
            Normal = 2,
            High = 3,
            Uregent = 4
        }*/
        public IAction Action { get; set; }
        public string Name { get; set; }
        public int MaxDegreeOfParallelism { get; set; }
        //private TaskPriority priority;
        public int Priority { get; set; } = 0;
        public DateTime? startTime;
        public DateTime? deadlineTime;
        public long? ExucutionTime { get; set; }

        public TaskSpecification()
        {

        }
        public TaskSpecification(IAction action, DateTime? startTime = null, DateTime? deadlineTime=null, long? exucutionTime =null,int maxDegreeOfParallelism = 1, int priority=0)
        {                     
            Priority = priority;
            this.Action = action;
            this.startTime = startTime; 
            this.deadlineTime = deadlineTime;
            this.ExucutionTime = exucutionTime;
            this.MaxDegreeOfParallelism = maxDegreeOfParallelism;            
        }
    }
}
