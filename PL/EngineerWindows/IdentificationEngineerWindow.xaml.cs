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

namespace PL.EngineerWindows
{
    /// <summary>
    /// Interaction logic for IdentificationEngineerWindow.xaml
    /// </summary>
    public partial class IdentificationEngineerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public class Source
        {
            public string EngineerId { get; set; }
        }

        Source sourceId = new Source() { EngineerId = "Enter id here..." };

        public IdentificationEngineerWindow()
        {

            try
            {
                DataContext = sourceId;
                InitializeComponent();
            }
            catch(Exception ex)
            {
                Tools.ErrorOccuredMesssage(ex.Message);
            }
            
        }
       
        /// <summary>
        /// enter to the window of a specific engineer that show its tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showDetailedTask_click(object sender, RoutedEventArgs e)
        {
            int id;
            bool success= int.TryParse(sourceId.EngineerId, out id);
            if (!success )
            {
                MessageBoxResult mbResult =
                MessageBox.Show("press OK to continue",
                 "id must contain only numbers, please insert another id");
                return;
            }
            else if(s_bl.Engineer.Read(id) is null)
            {
                MessageBoxResult mbResult =
                MessageBox.Show("press OK to continue",
                 "there is no engineer with such id, please insert another id");
                return;
            }
            new EngineerStartWindow(id).ShowDialog();
        }
    }
}
