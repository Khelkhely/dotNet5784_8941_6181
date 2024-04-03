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

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window 
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



    public DateTime MyClock
    {
        get { return (DateTime)GetValue(MyClockProperty); }
        set { SetValue(MyClockProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyClock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyClockProperty =
        DependencyProperty.Register("MyClock", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now.Date));


    public MainWindow()
    {
        InitializeComponent();
        //MyClock = s_bl.Clock;
    }

    private void EngineerMainWindow_Click(object sender, RoutedEventArgs e)
    {
        //ComboBox comboBox = new ComboBox();
        new EngineerIdWindow().Show();
    }

    private void ManagerMainWindow_Click(object sender, RoutedEventArgs e)
    {
        new ManagerMainWindow().Show();
    }

    private void AddYear_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AddYear();
        MyClock = s_bl.Clock;
    }
}