using PL.EngineerWindows;
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
    /// Interaction logic for DependenciesWindow.xaml
    /// </summary>
    public partial class DependenciesWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        int id; //id of task

        public DependenciesWindow(int? myId)
        {
            this.id = myId ?? 0;
            List<BO.TaskInList?> dependeciesTasksList = s_bl?.Task.Read(id)?.Dependencies ?? new List<BO.TaskInList?>();
            DependeciesTasksList = new List<BO.Task>();
           foreach (BO.TaskInList tl in dependeciesTasksList)
                {
                    BO.Task t = s_bl?.Task.Read(tl.Id);
                    if (t is null)
                    {
                        break;
                    }
                    else
                    {
                        DependeciesTasksList.Add(t);
                    }
           }
            InitializeComponent();

        }

        //list of the tasks
        public List<BO.Task> DependeciesTasksList
        {
            get { return (List<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("DependenciesTasksList", typeof(List<BO.Task>),
        typeof(TaskListWindow), new PropertyMetadata(null));

        //show only tasks that are not depends on the task now
        public bool AddDependecyFilter(BO.Task ? t)
        {
            if (t == null)
            {
                return false;

            }
            BO.Task task = t!;
            return true;
        }

        //adding a dependency
        private void AddDependency_onClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            new AddDependencyWindow(id).ShowDialog();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void levelFilter_selectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
