using BlApi;
using PL.Engineer;
using PL.Task;
using System.Text;
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
    /// Interaction logic for ManagerMainWindow.xaml
    /// </summary>
    public partial class ManagerMainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public ManagerMainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "Show engineers" button click event
        /// </summary>
        private void Button_Engineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show(); // Shows the list of engineers window
        }

        /// <summary>
        /// "Initialize data" button click event
        /// </summary>
        private void Button_Initialize_Click(object sender, RoutedEventArgs e)
        {
            //Initialize the data only if the user clicked "yes" in the Message box:
            if (MessageBox.Show("Are you sure you want to initialize data?",
                "Initialize data", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                s_bl.InitializeDB();
        }

        /// <summary>
        /// "Reset data" button click event
        /// </summary>
        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            //Reset the data only if the user clicked "yes" in the Message box:
            if (MessageBox.Show("Are you sure you want to reset data?",
                "Reset data", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                s_bl.ResetDB();
        }

        private void Button_Task_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }
    }
}