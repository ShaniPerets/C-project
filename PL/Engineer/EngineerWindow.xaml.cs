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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        static bool isAddingState = true;

        public EngineerWindow(int id = 0)
        {
            InitializeComponent();
            isAddingState = true;
            int Id = id; //id of current engineer
            if (Id != 0)
            {
                isAddingState = false;
            }
            //initialize current engineer according to its id. default id will make a default values engineer
            //if there is an error read an engineer, engineer will be the default values
            try
            {
                Engineer = (Id == 0) ? new BO.Engineer()
                {
                    Id = 0,
                    Name = "",
                    EMail = "",
                    Level = BO.EngineerExperience.Novice,
                    Cost = 0,
                    Task = null
                } : s_bl?.Engineer.Read(Id);

            }
            catch
            {
                Engineer = new BO.Engineer()
                {
                    Id = 0,
                    Name = "",
                    EMail = "",
                    Level = BO.EngineerExperience.Novice,
                    Cost = 0,
                    Task = null
                };
            }
        }

       
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("Engineer", typeof(BO.Engineer),
        typeof(EngineerWindow), new PropertyMetadata(null));

        
        //if the state is adding- add the negineer if user click the button. otherwise- update the engineer
        private void Add_Update_OnClick(object sender, RoutedEventArgs e)
        {
            if (isAddingState)
            {
                try
                {
                    s_bl?.Engineer.Create(Engineer);
                    Tools.successMesssage("engineer creation success");
                    this.Close();
                }
                catch(BO.BlInvalidInputException ex)
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
                    s_bl?.Engineer.Update(Engineer);
                    Tools.successMesssage("engineer updating success");
                    this.Close();
                }
                catch (BO.BlInvalidInputException ex)
                {
                    Tools.ErrorOccuredMesssage("Invalid details of engineer, please enter valid details");
                }
                catch(Exception ex)
                {
                    Tools.ErrorOccuredMesssage("Error While Updating Engineer");
                }
            }
        }
    }

}
