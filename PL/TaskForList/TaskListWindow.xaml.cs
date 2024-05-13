using PL.Engineer;
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

namespace PL.TaskForList
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.ReadAll();
        }

        //list of the tasks
        public IEnumerable<BO.Task> TaskList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>),
        typeof(TaskListWindow), new PropertyMetadata(null));

        //which level to filter the list of engineers by
        public BO.EngineerExperience Difficulty { get; set; } = BO.EngineerExperience.All;

        //filter the list of task by the level
        private void levelFilter_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Difficulty == BO.EngineerExperience.All) ?
            s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(t => t.difficulty == Difficulty)!;
        }

        private void AddTask_onClick(object sender, RoutedEventArgs e)
        {
            new TaskWindow ().ShowDialog();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //show single task window
        private void SingleEngWindow_onDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Task? t = (sender as ListView)?.SelectedItem as BO.Task;

            new TaskWindow(t.Id).ShowDialog();
        }
    }
}
