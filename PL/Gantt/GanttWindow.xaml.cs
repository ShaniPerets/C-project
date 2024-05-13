using System;
using System.Collections.Generic;
using System.Data;
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

namespace PL.Gantt
{
    /// <summary>
    /// Interaction logic for GanttWindow.xaml
    /// </summary>
    public partial class GanttWindow : Window
    {
        //list of the table
        public List<GanttRecord> MyData { get; set; }
        public GanttWindow()
        {
            InitializeComponent();

            MyPropety = new();
            MyData = new List<GanttRecord>();
        }
        public SelectTaskToGantt MyPropety
        {
            get { return (SelectTaskToGantt)GetValue(MyPropetyProperty); }
            set { SetValue(MyPropetyProperty, value); }
        }
        public static readonly DependencyProperty MyPropetyProperty =
            DependencyProperty.Register("MyPropety", typeof(SelectTaskToGantt), typeof(GanttWindow), new PropertyMetadata(null));

        //creating a daynamic datatable according to the tasks deatils and their status
        public class SelectTaskToGantt
        {
            static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
            public DataTable Entries { get; set; }

            public SelectTaskToGantt()
            {
                List<BO.Task> tasks = s_bl.Task.ReadAll().ToList();

                Entries = new DataTable()
                {
                    Columns =
                    {
                        new DataColumn("Task Id", typeof (string)),
                        new DataColumn("Task Name", typeof (string)),
                        new DataColumn("Dependencies", typeof (string))
                    }
                };
                DateTime? min = s_bl.Clock;
                DateTime? max = s_bl.Clock;

                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].BeginWorkDateP > max)
                        max = tasks[i].BeginWorkDateP;
                    if (tasks[i].CreateTime < min)
                        min = tasks[i].CreateTime;
                }
                if (min == null || max == null)
                    MessageBox.Show("There is no date for the project, can't open gunt", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    for (DateTime d = min!.Value; d <= max; d = d.AddDays(1))
                    {
                        string str = $"{d.Day}-{d.Month}-{d.Year}";
                        DataColumn ro = new(str, typeof(string));
                        Entries.Columns.Add(ro);
                    }
                    for (int i = 0; i < tasks.Count(); i++)
                    {
                        BO.Task task = tasks[i];
                        DataRow row = Entries.NewRow();
                        row[0] = task.Id;
                        row[1] = task.Name;
                        List<BO.TaskInList> d = task.Dependencies!;
                        string str = "(";
                        for (int j = 0; j < d.Count; j++)
                        {
                            str += $"{d[j].Id},";
                        }
                        str += ")";
                        row[2] = str;
                        int x = 3;
                        for (DateTime day = min.Value; day <= max!.Value; day = day.AddDays(1), x++)
                        {
                            if (task.BeginWorkDate != null)
                            {
                           
                                if ((day < task.BeginWorkDate))
                                {
                                    string strToPut = "Scheduled";
                                    row[x] = strToPut;
                                }

                                else if ((day > task.BeginWorkDate) && (day < task.EndWorkTime))
                                {
                                    string strToPut = "OnTrack";
                                    row[x] = strToPut;
                                }
                                else if (day > task.EndWorkTime)
                                {
                                    string strToPut = "Done";
                                    row[x] = strToPut;
                                }
                            }
                            else
                            {
                                string strToPut = "Unscheduled";
                                row[x] = strToPut;
                            }
                        }
                        Entries.Rows.Add(row);
                    }
                }


            }
        }
    }
}

