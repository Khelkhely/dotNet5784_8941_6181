using DalApi;
using PL.Task;
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

namespace PL;

/// <summary>
/// Interaction logic for EngineerMainWindow.xaml
/// </summary>
public partial class EngineerMainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.Task? MyTask
    {
        get { return (BO.Task)GetValue(myTaskProperty); }
        set { SetValue(myTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty myTaskProperty =
        DependencyProperty.Register("MyTask", typeof(BO.Task), typeof(EngineerMainWindow));

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(EngineerMainWindow));


    BO.Engineer myEng = new BO.Engineer();

    public EngineerMainWindow(int engineerId = 0)
    {      
        InitializeComponent();

        try
        {
            myEng = s_bl.Engineer.Read(engineerId);
            if (myEng.Task != null) MyTask = s_bl.Task.Read(myEng.Task.Id);
            else
            {

                TaskList = s_bl.Task.GetTaskList(task => task.Engineer == null && !(task.Copmlexity > myEng.Level));
                //לא מבוצעות על ידי מהנדס אחרV
                //אין משימות קודמות שלא הסתיימו
                //אותה רמה או רמה נמוכה יותרV
                //יש פונקציה חדשה בbl בשביל זה
            }

        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private void FinishTaskButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.finishTask(myEng.Id, MyTask!.Id);
        MyTask = null;
    }

    private void TaskSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        int id = ((sender as ListView)!.SelectedItem as BO.TaskInList)!.Id;
        MyTask = s_bl.Task.Read(id);
        s_bl.AssignEngineer(myEng.Id, MyTask!.Id);
        s_bl.startNewTask(myEng.Id, MyTask!.Id);
    }
}
