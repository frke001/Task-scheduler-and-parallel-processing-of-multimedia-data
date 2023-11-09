using Raspoređivač_zadataka;
using Raspoređivač_zadataka.Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskScheduler = Raspoređivač_zadataka.TaskScheduler;

namespace GUI
{
    /// <summary>
    /// Interaction logic for CreateTaskWindow.xaml
    /// </summary>
    public partial class CreateTaskWindow : Window
    {
        public TaskScheduler taskScheduler;
        public CreateTaskWindow(TaskScheduler taskScheduler)
        {
            InitializeComponent();
            this.taskScheduler = taskScheduler;
            List<Type> taskTypes = typeof(IAction).Assembly.GetTypes().Where((type) =>
                    type.GetInterfaces().Contains(typeof(IAction))).Select(t => t).ToList();
            maxDegreeOfParallelismComboBox.ItemsSource = Enumerable.Range(1, Environment.ProcessorCount);
            //List<string> taskTypesNames = new List<string>();
            // taskTypes.ForEach((j) => taskTypesNames.Add(j.Name));
            taskTypeListBox.ItemsSource = taskTypes;
            taskTypeListBox.SelectedIndex = 0;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            return;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(nameTextBox.Text) && !String.IsNullOrEmpty(maxDegreeOfParallelismComboBox.Text))
            {
                TaskSpecification taskSpecification = new TaskSpecification();
                taskSpecification.Name = nameTextBox.Text;
                taskSpecification.MaxDegreeOfParallelism = int.Parse(maxDegreeOfParallelismComboBox.Text);
                try
                {
                    if (!String.IsNullOrEmpty(priorityTextBox.Text))
                    {
                        int priority = int.Parse(priorityTextBox.Text);
                        taskSpecification.Priority = priority;
                    }
                    if(!String.IsNullOrEmpty(executionTimeTextBox.Text))
                    {
                        int executionTime = int.Parse(executionTimeTextBox.Text);
                        taskSpecification.ExucutionTime = executionTime;
                    }
                    if ((bool)startTimeCheckBox.IsChecked)
                    {
                        if (startTimeDatePicker.SelectedDate.HasValue && !String.IsNullOrEmpty(startTimeTextBox.Text))
                        {

                            taskSpecification.startTime = GetDateTime(startTimeDatePicker,startTimeTextBox);
                        }
                    }
                    if ((bool)deadlineCheckBox.IsChecked)
                    {
                        if (deadlineDatePicker.SelectedDate.HasValue && !String.IsNullOrEmpty(deadlineTextBox.Text))
                        {

                            taskSpecification.deadlineTime = GetDateTime(deadlineDatePicker,deadlineTextBox);
                        }
                    }
                    Type taskType = (Type)taskTypeListBox.SelectedItem;
                    userTaskJsontextBox.Text = userTaskJsontextBox.Text.Replace("\\", "\\\\");
                    IAction userTask = (IAction)Activator.CreateInstance(taskType);
                    taskSpecification.Action = (IAction)JsonSerializer.Deserialize(userTaskJsontextBox.Text, userTask.GetType(), jsonSerializerOptions);                   
                    taskScheduler.ScheduleWithoutStart(taskSpecification);
                    this.Close();
                }               
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid parameters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Invalid parameters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private DateTime GetDateTime(DatePicker datePicker,TextBox textBox)
        {
            DateTime selectedDate = datePicker.SelectedDate.Value;
            string[] time = textBox.Text.Split(":");
            int hours = int.Parse(time[0]);
            int minutes = int.Parse(time[1]);
            int seconds = int.Parse(time[2]);
            return new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hours, minutes, seconds);

        }
        private List<Type> getTypes()
        {
            return typeof(IAction).Assembly.GetTypes().Where((type) =>
                    type.IsSubclassOf(typeof(IAction))).ToList();
        }

        private void TaskTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Type taskType = (Type)taskTypeListBox.SelectedItem;
            object userTask = Activator.CreateInstance(taskType);
            userTaskJsontextBox.Text = JsonSerializer.Serialize(userTask, userTask.GetType(), jsonSerializerOptions);
        }
        private static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true, IncludeFields = true };
    }
}
