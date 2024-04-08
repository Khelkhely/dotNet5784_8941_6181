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

namespace PL;

/// <summary>
/// Interaction logic for ManagerMainWindow.xaml
/// </summary>
public partial class ManagerMainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public bool IsSchedule
    {
        get { return (bool)GetValue(IsScheduleProperty); }
        set { SetValue(IsScheduleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsSchedule.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsScheduleProperty =
        DependencyProperty.Register("IsSchedule", typeof(bool), typeof(ManagerMainWindow), new PropertyMetadata(false));

    public ManagerMainWindow()
    {
        if (s_bl.GetStartDate() != null)
            IsSchedule = true;
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

    private void Button_Gantt_Click(object sender, RoutedEventArgs e)
    {
        new GanttChartWindow().ShowDialog();
    }
    private void Button_CreateSchedule_Click(object sender, RoutedEventArgs e)
    {
        new CreateScheduleWindow().ShowDialog();
        if (s_bl.GetStartDate() != null)
            IsSchedule = true;
    }
    
}