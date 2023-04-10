using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    public class Resource
    {
        private static Graph graph = new();
        private string resourceName;
        private int? ownerPriority = null; // we remember the old priority value if it has been changed
        public string ResourceName { get { return resourceName; }  set { this.resourceName = value; } }
        private Task? owner;
        private PriorityQueue<Task, int> waitingForResource = new PriorityQueue<Task, int>(Comparer<int>.Create((x, y) => y - x)); // who is waiting for the resource

        public Resource(string resourceName)
        {
            this.resourceName = resourceName;
            this.owner = null;
        }
        
        public void TryLock(Task task) // PCP
        {
            bool status = false;
            lock (this)
            {
                if(this.owner != null) // resource taken
                {
                    graph.TryAddTransition(task, owner); // tsk waiting for owner
                    this.waitingForResource.Enqueue(task,task.TaskSpecification.Priority); // the branch is directed from the waiting task to the waiting task to release the resource
                    if (task.TaskSpecification.Priority > this.owner.TaskSpecification.Priority) // priority inversion, the priority of smaller tasks is remembered
                                                                                                 // and it is set to the priority of the one who wants the resource
                    {
                        ownerPriority = owner.TaskSpecification.Priority; // we save old priority
                        owner.TaskSpecification.Priority = task.TaskSpecification.Priority; // and we give higher priority to the current owner of the resource
                    }
                    // Inheritance - Related Inversion
                    /*
                     * Suppose there are three tasks T1, T2 and T3. The priority of these tasks is as T1 > T2 > T3. 
                     * T1 and T3 both need the critical resource CR while T2 has no requirement of critical resource. 
                     * At some point of time, T3 (lowest priority task) acquires the critical resource CR.
                     * Now T1 (highest priority task) needs the critical resource CR and requests for it. 
                     * Now by inheritance clause T3 gets its priority equal to priority of T1. 
                     * Therefore, T2, which do not need the critical resource, do not get executed as a result of raised priority of T3. 
                     * Hence task T2 goes under the inheritance-related inversion.
                     */ 
                    status = true;
                }
                else
                { // resource is not taken
                    this.owner = task;
                }
            }
            if (status) // to avoid a lock in a lock, because if the resource is occupied, the task that requested it must wait for the resource, similar to pause, wait
            {
                task.BlockForResourse(); // the task stops execution
                this.owner = task; // it becomes the new owner
            }
        }

        public void Unlock()
        {
            lock (this)
            {
                if (owner != null) // if the resource is busy, we release it and give it to the next task if there is one
                {
                    if (this.ownerPriority != null) // if != null, it means that the priority was changed and we return it, remembered
                    {
                        this.owner.TaskSpecification.Priority = this.ownerPriority.Value; // let's return the remembered priority value
                        this.ownerPriority = null;
                    }
                    this.owner = null;

                    if (waitingForResource.Count != 0) // if someone is waiting for a resource, the next in line is taken
                    {
                        Task task = waitingForResource.Dequeue();
                        graph.RemoveTransition(task); // that task is no longer waiting, a branch from it does not exist
                        task.UnblockForResource(); // we call the handler to continue the task
                    }
                }
            }
        }

    }
}
