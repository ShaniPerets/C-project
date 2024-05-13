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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        //observer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll();
        }

        //list of the engineers
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>),
        typeof(EngineerListWindow), new PropertyMetadata(null));

        //which level to filter the list of engineers by
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.All;

        //change the engineer list accordding to the filter that chose
        private void levelFilter_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Level == BO.EngineerExperience.All) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(e => e.Level == Level)!;
        }

        //show the engineer window to fill deatils and add
        private void AddEngineer_onClick(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //show the engineer window
        private void SingleEngWindow_onDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? eng = (sender as ListView)?.SelectedItem as BO.Engineer;

            new EngineerWindow(eng.Id).ShowDialog();
        }

        //when return to the window after moving to another window- refresh the list of engineers.
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            EngineerList = s_bl?.Engineer.ReadAll();
        }
    }
}
