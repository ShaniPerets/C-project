using PL.Engineer;
using PL.EngineerWindows;
using PL.Gantt;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            s_bl.InitClock();
            Clock = s_bl.Clock;
            InitializeComponent();
        }

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// move to the engineers list window. show all engineers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showEngineerList_click(object sender, RoutedEventArgs e)
        { 
            new AdminStartWindow().ShowDialog();
        }

        private void showSingleEngineer_click(object sender, RoutedEventArgs e)
        {
            
            new IdentificationEngineerWindow().ShowDialog();
        }

       

        /// <summary>
        /// initialize the db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initDB_click(object sender, RoutedEventArgs e)
        {
            //ask the user if hw want to initialize the DB
            MessageBoxResult toInit =
            MessageBox.Show("Do you want to initialize the database?", "init",
            MessageBoxButton.YesNo, MessageBoxImage.Question
            );

            //user do not want to initialize db
            if (toInit == MessageBoxResult.No) { return; }

            //user want to initialize db:
            s_bl.InitializeDB();


        }

        /// <summary>
        /// reset the data base and remove all its content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetDB_click(object sender, RoutedEventArgs e)
        {
            //ask the user if hw want to reset the DB
            MessageBoxResult toReset =
            MessageBox.Show("Do you want to reset the database?", "reset",
            MessageBoxButton.YesNo, MessageBoxImage.Question
            );

            //user do not want to initialize db
            if (toReset == MessageBoxResult.No) { return; }

            //user want to initialize db:
            s_bl.ResetDB();
        }

        //current clock
        public DateTime Clock
        {
            get { return (DateTime)GetValue(ClockProperty); }
            set { SetValue(ClockProperty, value); }
        }

        public static readonly DependencyProperty ClockProperty =
        DependencyProperty.Register("Clock", typeof(DateTime),
        typeof(MainWindow), new PropertyMetadata(null));

        //add hour button
        private void AddHour_onClick(object sender, RoutedEventArgs e)
        {
            s_bl.AddHourClock();
            Clock = Clock.AddHours(1);
        }
        //add day button
        private void AddDay_onClick(object sender, RoutedEventArgs e)
        {
            s_bl.AddDayClock();
            Clock = Clock.AddDays(1);
        }

        //active the clock
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Clock = s_bl.Clock;
        }
    }
}

