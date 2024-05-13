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
    /// Interaction logic for AddDependencyWindow.xaml
    /// </summary>
    public partial class AddDependencyWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        int id = 0;

        public AddDependencyWindow(int id = 0)
        {
            this.id = id;
            TaskssList = s_bl?.Task.ReadAll(Filter);
            InitializeComponent();

        }

        //list of the tasks
        public IEnumerable<BO.Task> TaskssList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskssList", typeof(IEnumerable<BO.Task>),
        typeof(TaskListWindow), new PropertyMetadata(null));


        //give the task window with option to choose a dependency
        private void SingleEngWindow_onDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Task? t = (sender as ListView)?.SelectedItem as BO.Task;

            try
            {
                MessageBoxResult mbResult =
                    MessageBox.Show("press OK to confirm, else press Cancel",
                     "are you sure you want this task?",
                    MessageBoxButton.OKCancel
                    );

                switch (mbResult)
                {
                    case MessageBoxResult.OK:
                        s_bl.Task.addDependency(t.Id, id);
                        Tools.successMesssage("add dependency success");
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }

            }

            catch (BO.BlUnAuthrizedAccessExpetion ex)
            {
                Tools.ErrorOccuredMesssage("you unautorize to add a dependency");
            }
            catch (BO.BlLoopDependencyExpetion ex)
            {
               Tools.ErrorOccuredMesssage("cant make a loop dependency");
            }
            catch (Exception ex) 
            {
                Tools.ErrorOccuredMesssage("Error While Adding a Dependency Task, make sure the dependency is vaild");
            }

        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //show only tasks that are not depends on the task now
        public bool Filter(BO.Task t)
        {
            if (t == null)
            {
                return false;

            }
            if (t.Dependencies != null && t.Dependencies.Where(td => td.Id == t.Id).ToList().Count() > 0 || id == t.Id)
            {
                return false;
            }
            if (t.Dependencies?.Select(t => t.Id).ToList().Contains(t.Id) == true) //not make loop dependencies
            {
                return false;
            }
            return true;
        }

    }
}
