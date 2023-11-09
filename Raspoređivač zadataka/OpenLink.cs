using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    public class OpenLink : IAction
    {
        public string LinkToOpen { get; set; }

        public OpenLink() { }
        public OpenLink(string linkToOpen)
        {
            this.LinkToOpen = linkToOpen;
        }
        public void Run(ICoOperative taskApi)
        {
            try
            {
                Process.Start(new ProcessStartInfo(LinkToOpen) { UseShellExecute = true });
                taskApi.SetProgress(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Došlo je do greške prilikom otvaranja linka: {ex.Message}");
            }
        }
    }
}
