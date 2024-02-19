using BlApi;
using PL.Engineer;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Engineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
        private void Button_Initialize_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to initialize data?", 
                "Initialize data", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                s_bl.InitializeDB();
        }
        
        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset data?",
                "Reset data", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                s_bl.ResetDB();
        }
    }
}