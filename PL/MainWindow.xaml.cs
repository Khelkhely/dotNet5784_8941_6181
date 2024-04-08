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

    public bool IsSchedule
    {
        get { return (bool)GetValue(IsScheduleProperty); }
        set { SetValue(IsScheduleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsSchedule.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsScheduleProperty =
        DependencyProperty.Register("IsSchedule", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

    public MainWindow()
    {
        if (s_bl.IsScheduled())
            IsSchedule = true;
        InitializeComponent();
    }

    /// <summary>
    /// "Engineer" button click event
    /// </summary>
    private void EngineerMainWindow_Click(object sender, RoutedEventArgs e)
    {
        new EngineerIdWindow().Show();
    }

    /// <summary>
    /// "Manager" button click event
    /// </summary>
    private void ManagerMainWindow_Click(object sender, RoutedEventArgs e)
    {
        new ManagerMainWindow().ShowDialog();
        if (s_bl.IsScheduled())
            IsSchedule = true;
    }

    /// <summary>
    /// "Add Year" button click event
    /// </summary>
    private void AddYear_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AddYear();
        MyClock = s_bl.Clock;
    }

    /// <summary>
    /// "Add Month" button click event
    /// </summary>
    private void AddMonth_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AddMonth();
        MyClock = s_bl.Clock;
    }

    /// <summary>
    /// "Add Day" button click event
    /// </summary>
    private void AddDay_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AddDay();
        MyClock = s_bl.Clock;
    }
}