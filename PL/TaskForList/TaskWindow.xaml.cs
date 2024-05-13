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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
            static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

            static bool isAddingState = true;

            public TaskWindow(int id = 0)
            {
                InitializeComponent();
                isAddingState = true;
                int Id = id; //id of current task
                if (Id != 0)
                {
                    isAddingState = false;
                }
                //initialize current engineer according to its id. default id will make a default values engineer
                //if there is an error read an engineer, engineer will be the default values
                try
                {
                    Task = (Id == 0) ? new BO.Task()
                    {
                        Id = 0,
                        Name = " ",
                        difficulty = BO.EngineerExperience.Novice,
                        StatusTask = BO.Status.Unscheduled,
                        Dependencies = null,
                        TaskDescription = " ",
                        Product =" ",
                        Comments = " ",
                        CreateTime = null,
                        BeginWorkDate = null,
                        BeginWorkDateP = null,
                        WorkDuring = 0,
                        DeadLine = null,
                        EndWorkTime = null,
                        EngineerId = null,
                        Engineer =null

                    } : s_bl?.Task.Read(Id);

                }
                catch
                {
                    Task = new BO.Task()
                    {

                        Id = 0,
                        Name = " ",
                        difficulty = BO.EngineerExperience.Novice,
                        StatusTask = BO.Status.Unscheduled,
                        Dependencies = null,
                        TaskDescription = " ",
                        Product = " ",
                        Comments = " ",
                        CreateTime = null,
                        BeginWorkDate = null,
                        BeginWorkDateP = null,
                        WorkDuring = 0,
                        DeadLine = null,
                        EndWorkTime = null,
                        EngineerId = null,
                        Engineer = null

                    };
                }



            }


            public BO.Task Task
            {
                get { return (BO.Task)GetValue(TaskProperty); }
                set { SetValue(TaskProperty, value); }
            }

            public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task),
            typeof(TaskWindow), new PropertyMetadata(null));

            //if the state is adding- add the negineer if user click the button. otherwise- update the engineer
            private void Add_Update_OnClick(object sender, RoutedEventArgs e)
            {
                if (isAddingState)
                {
                    try
                    {
                        s_bl?.Task.Create(Task);
                        Tools.successMesssage("engineer creation success");
                        this.Close();
                    }
                    catch (BO.BlInvalidInputException ex)
                    {
                       Tools.ErrorOccuredMesssage("Invalid details of engineer, please enter valid details");
                    }
                    catch (BO.BlAlreadyExistsException ex)
                    {
                        Tools.ErrorOccuredMesssage("engineer is already exist in the system. please try another one");
                    }
                    catch
                    {
                        Tools.ErrorOccuredMesssage("Error While Creating Engineer");
                    }

                }
                else
                {
                    try
                    {
                    
                        s_bl?.Task.Update(Task);
                        Tools.successMesssage("engineer updating success");
                        this.Close();
                    }
                    catch (BO.BlInvalidInputException ex)
                    {
                        Tools.ErrorOccuredMesssage("Invalid details of engineer, please enter valid details");
                    }
                    catch
                    {
                        Tools.ErrorOccuredMesssage("Error While Updating Engineer");
                    }
                }
            }

        //show dependency window
        private void ShowDependencies_OnClick(object sender, RoutedEventArgs e)
        {
            new DependenciesWindow(Task.Id).ShowDialog();
        }
    }
    }
