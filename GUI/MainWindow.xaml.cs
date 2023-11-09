using Raspoređivač_zadataka.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskScheduler = Raspoređivač_zadataka.TaskScheduler;
using Task = Raspoređivač_zadataka.Task;
using TaskSpecification = Raspoređivač_zadataka.TaskSpecification;
using System.Collections.ObjectModel;
using Raspoređivač_zadataka;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TaskScheduler taskScheduler;
        public MainWindow(int maxNumberOfConcurrentTasks, SchedulingAlgorithm schedulingAlgorithm,int timeSlice)
        {
            InitializeComponent();
            taskScheduler = new TaskScheduler(maxNumberOfConcurrentTasks,schedulingAlgorithm);
            //taskScheduler.AddResource(new Resource("res1"));
            //taskScheduler.AddResource(new Resource("res2"));
            tasksListView.ItemsSource = taskScheduler.Tasks;
        }
       

        private void CreateJobButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskWindow createTaskWindow = new CreateTaskWindow(taskScheduler);
            createTaskWindow.Show();
            
        }
        private void StartTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // sender is the component that calls the method

            Task task = (Task)(((Button)sender).DataContext);
            try
            {
                lock (task.taskLock)
                {
                    if (task.State.Equals(Task.TaskState.ScheduledWithoutStarting))
                        task.StartScheduling();
                    else if (task.State.Equals(Task.TaskState.Paused))
                        task.RequestContinue();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }        
        private void PauseTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = (Task)(((Button)sender).DataContext);
            task.RequestPause();
        }
        
        private void StopTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = (Task)(((Button)sender).DataContext);
            task.RequestTerminate();
        }

        private void CloseTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = (Task)(((Button)sender).DataContext);
            task.RequestTerminate();
            taskScheduler.Tasks.Remove(task);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close the application?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {             
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double availableWidth = e.NewSize.Width;


            NameColumn.Width = availableWidth/6;
            StateColumn.Width = availableWidth / 4;
            ProgressColumn.Width = availableWidth / 3;
           

            ControlsColumn.Width = availableWidth / 7;
            
        }
        
    }
}
