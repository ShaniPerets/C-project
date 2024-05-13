using PL.TaskForList;
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

namespace PL.EngineerWindows
{
    /// <summary>
    /// Interaction logic for TaskOfEngineerListWindow.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>

    public partial class TaskOfEngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        Action<BO.Task?> doubleClick_method = null;
        Func<BO.Task, bool> filter = null;
        bool isManager = false;

        public TaskOfEngineerListWindow(Func<BO.Task?, bool> filter = null, Action<BO.Task?> doubleClick_method = null, bool isManager = false) {
            this.isManager = isManager;
            TasksList = s_bl?.Task.ReadAll(filter);
            this.doubleClick_method = doubleClick_method;
            this.filter = filter;
            InitializeComponent();
            
        }

        //list of the tasks
        public IEnumerable<BO.Task> TasksList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TasksList", typeof(IEnumerable<BO.Task>),
        typeof(TaskListWindow), new PropertyMetadata(null));

        //show task deatils
        private void SingleEngWindow_onDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Task? t = (sender as ListView)?.SelectedItem as BO.Task;

            doubleClick_method(t);

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
