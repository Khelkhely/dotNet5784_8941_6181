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
        DependencyProperty.Register("Starting", typeof(DateTime), typeof(CreateScheduleWindow));


    public DateTime? Date
    {
        get { return (DateTime?)GetValue(dateProperty); }
        set { SetValue(dateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for date.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty dateProperty =
        DependencyProperty.Register("date", typeof(DateTime?), typeof(CreateScheduleWindow), new PropertyMetadata(null));



    public bool Flag
    {
        get { return (bool)GetValue(flagProperty); }
        set { SetValue(flagProperty, value); }
    }

    // Using a DependencyProperty as the backing store for flag.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty flagProperty =
        DependencyProperty.Register("flag", typeof(bool), typeof(CreateScheduleWindow), new PropertyMetadata(false));


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
        DependencyProperty.Register("MyProperty", typeof(IEnumerable<BO.TaskInList>), typeof(CreateScheduleWindow), new PropertyMetadata(null));


    public CreateScheduleWindow()
    {
        try
        {
            TaskList = s_bl.Task.GetTaskList(task => task.ScheduledDate is null);
        }
        catch (BO.BlDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        InitializeComponent();
    }

    private void TaskSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Flag = true;
            int id = ((sender as ListView)!.SelectedItem as BO.TaskInList)!.Id;
            MyTask = s_bl.Task.Read(id);

        }
        catch (BO.BlDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }



    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        // if (Date == null)
        s_bl.Task.UpdateTaskDate(MyTask!.Id, (DateTime)Date, Starting);
        TaskList = s_bl.Task.GetTaskList(task => task.ScheduledDate is null);
        if (TaskList.Count() == 0)
        {
            s_bl.CreateSchedule(Starting);
            Close();
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}
