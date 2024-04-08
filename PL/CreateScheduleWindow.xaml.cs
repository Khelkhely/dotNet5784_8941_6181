using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL;

/// <summary>
/// Interaction logic for CreateScheduleWindow.xaml
/// </summary>
public partial class CreateScheduleWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public DateTime Starting
    {
        get { return (DateTime)GetValue(StartingProperty); }
        set { SetValue(StartingProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Starting.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StartingProperty =
        DependencyProperty.Register("Starting", typeof(DateTime), typeof(CreateScheduleWindow), new PropertyMetadata(DateTime.Today));

    public DateTime? Date
    {
        get { return (DateTime?)GetValue(DateProperty); }
        set { SetValue(DateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for date.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register("Date", typeof(DateTime?), typeof(CreateScheduleWindow), new PropertyMetadata(null));

    public bool Flag
    {
        get { return (bool)GetValue(FlagProperty); }
        set { SetValue(FlagProperty, value); }
    }

    // Using a DependencyProperty as the backing store for flag.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FlagProperty =
        DependencyProperty.Register("Flag", typeof(bool), typeof(CreateScheduleWindow), new PropertyMetadata(false));

    public BO.Task? MyTask
    {
        get { return (BO.Task?)GetValue(MyTaskProperty); }
        set { SetValue(MyTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyTaskProperty =
        DependencyProperty.Register("MyTask", typeof(BO.Task), typeof(CreateScheduleWindow), new PropertyMetadata(null));

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(CreateScheduleWindow), new PropertyMetadata(null));


    public CreateScheduleWindow()
    {
        try
        {
            TaskList = s_bl.Task.GetTaskList(task => task.ScheduledDate is null);
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        InitializeComponent();
    }

    /// <summary>
    /// Task Selected event
    /// </summary>
    private void TaskSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Flag = true;
            int? id = ((sender as ListView)!.SelectedItem as BO.TaskInList)?.Id;
            if (id != null)
                MyTask = s_bl.Task.Read((int)id);
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// "Update" button click event
    /// </summary>
    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {

        if (Date is DateTime)
        {
            try
            {
                s_bl.Task.UpdateTaskDate(MyTask!.Id, (DateTime)Date!, Starting);
                TaskList = s_bl.Task.GetTaskList(task => task.ScheduledDate is null);
                if (TaskList.Count() == 0)
                {
                    s_bl.CreateSchedule(Starting);
                    Close();
                }
                Date = null;
                Flag = false;
            }
            catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
            MessageBox.Show("The date entered is incorrect. Pleas try again");

    }
}
