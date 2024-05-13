using BO;
using PL.Engineer;
using PL.EngineerWindows;
using PL.TaskForList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for EngineerStartWindow.xaml
    /// </summary>
    public partial class EngineerStartWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public int Id;

        public EngineerStartWindow(int? id)
        {
            try
            {
               
                Id = id ?? 0;
                Engineer = s_bl.Engineer.Read(Id) ?? throw new Exception("there is no such engineer");
                //task the engineer work on currently
                BO.Task ? engCurrentTask = s_bl.Task.ReadAll(t=>t.EngineerId == Id && t.StatusTask != Status.Done).FirstOrDefault();

                //if engineer does not have a task to fulfill he can choose one
                if (engCurrentTask != null)
                {
                    IsFinished = false;
                }
                else
                {
                    IsFinished = true;
                }
                
                InitializeComponent();

            }
            catch (Exception ex) {
                Tools.ErrorOccuredMesssage(ex.Message);
            }
        }


        //current engineer
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("Engineer", typeof(BO.Engineer),
        typeof(EngineerWindow), new PropertyMetadata(null));

        //is the engineer fullfilled its task
        public bool IsFinished
        {
            get { return (bool)GetValue(IsFinishedProperty); }
            set { SetValue(IsFinishedProperty, value); }
        }

        public static readonly DependencyProperty IsFinishedProperty =
        DependencyProperty.Register("IsFinished", typeof(bool),
        typeof(EngineerWindow), new PropertyMetadata(null));

        public bool Filter(BO.Task? t)
        {
            if (t?.EngineerId is null)
            {
                return true;
            }
            return false;
        }
        //let the engineer the approtoninty to choose a task
        public void ChoosingTask(BO.Task? t)
        {
            try
            {
                t.EngineerId = Id;

                MessageBoxResult mbResult =
                    MessageBox.Show("press OK to confirm, else press Cancel",
                     "are you sure you want this task?",
                    MessageBoxButton.OKCancel
                    );

                switch (mbResult)
                {
                    case MessageBoxResult.OK:
                        s_bl?.Task.Update(t);
                        Tools.successMesssage("engineer updating success");
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }

            }
            catch
            {
                Tools.ErrorOccuredMesssage("Error While Choosing Task of Engineer");
            }
        }
        //show the current engineer tasks list
        private void showTaskList_click(object sender, RoutedEventArgs e)
        {
            if(IsFinished == true)
            {
                this.Close();
                new TaskOfEngineerListWindow(Filter, ChoosingTask).ShowDialog();
            }
            else
            {
                Tools.ErrorOccuredMesssage("choosing a task valid only after current task has been fulfilled");
            }
        }

        //show the task details
        private void showDetailedTask_click(object sender, RoutedEventArgs e)
        {
            if (!IsFinished)
            {
                int tId = Engineer.Task?.Id ?? 0;
                new TaskWindow(tId).ShowDialog();
            }
            else
            {
                Tools.ErrorOccuredMesssage("you do not have a task to fulfill");
            }
        }

    }
}
