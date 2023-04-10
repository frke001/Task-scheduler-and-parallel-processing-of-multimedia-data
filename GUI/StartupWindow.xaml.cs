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
using System.Windows.Shapes;
using Raspoređivač_zadataka.Algorithms;
namespace GUI
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;

            List<Type> algorithms = getTypes();
            List<string> names = new List<string>();
            algorithms.ForEach((alg) => names.Add(alg.Name));
            schedulingAlgorithmComboBox.ItemsSource = names;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (schedulingAlgorithmComboBox != null && maxNumberOfConcurentTasksTextBox.Text != "")
            {
                try
                {
                    int maxNumOfConTask = int.Parse(maxNumberOfConcurentTasksTextBox.Text.ToString());
                    int timeSlice = int.Parse(timeSliceTextBox.Text.ToString());
                    SchedulingAlgorithm algorithm = null;

                    List<Type> algorithms = getTypes();
                    algorithms.ForEach((alg) =>
                    {
                        if (alg.Name.Equals(schedulingAlgorithmComboBox.Text))
                        {
                            
                            algorithm = (SchedulingAlgorithm)Activator.CreateInstance(alg);
                            if (algorithm is TimeSlicesSchedulingAlgorithm)
                                ((TimeSlicesSchedulingAlgorithm)algorithm).timeSlice = int.Parse(timeSliceTextBox.Text);
                        }
                    });

                    if (maxNumOfConTask != 0 && algorithm != null)
                    {
                        MainWindow wimainWindown = new MainWindow(maxNumOfConTask, algorithm, timeSlice);
                        this.Hide();
                        wimainWindown.Show();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid parameters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private List<Type> getTypes()
        {
            return typeof(SchedulingAlgorithm).Assembly.GetTypes().Where((type) =>
                    type.IsSubclassOf(typeof(SchedulingAlgorithm))).ToList();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
