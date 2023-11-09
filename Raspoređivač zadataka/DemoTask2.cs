using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    public class DemoTask2 : IAction
    {
        public int NumOfIterations { get; set; } = 5;
        public DemoTask2()
        {

        }

        public void Run(ICoOperative taskApi)
        {

            Console.WriteLine(" started");
            int i = 0;
            for (i = 0; i < NumOfIterations; i++)
            {
                taskApi.SetProgress(i / (float)NumOfIterations);

                Console.WriteLine(" running: " + i);
                taskApi.CheckForPause();
                taskApi.CheckForTermination();
                taskApi.CheckForContextSwitch();
                Thread.Sleep(1000);
                taskApi.CheckForPause();
                taskApi.CheckForTermination();
                taskApi.CheckForContextSwitch();
            }
            taskApi.SetProgress(i / (float)NumOfIterations);
            taskApi.CheckForPause();
            taskApi.CheckForTermination();
            taskApi.CheckForContextSwitch();
            Console.WriteLine(" finished");
        }

    }
}
