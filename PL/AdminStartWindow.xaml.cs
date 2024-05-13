using PL.Engineer;
using PL.Gantt;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminStartWindow.xaml
    /// </summary>
    public partial class AdminStartWindow : Window
    {
        public AdminStartWindow()
        {
            InitializeComponent();
        }

        //button to the engineer list window
        private void showEngineerList_click(object sender, RoutedEventArgs e)
        { new EngineerListWindow().ShowDialog(); }

        //button to the tasks list window

        private void showTaskList_click(object sender, RoutedEventArgs e)
        { new TaskListWindow().ShowDialog(); }

        //button to the gantt window
        private void showGaant_click(object sender, RoutedEventArgs e)
        {
            new GanttWindow().ShowDialog();
        }
    }
}
