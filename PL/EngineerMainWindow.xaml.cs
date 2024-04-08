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
        DependencyProperty.Register("MyTask", typeof(BO.Task), typeof(EngineerMainWindow), new PropertyMetadata(null));

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(EngineerMainWindow));



    public bool IsEmpty
    {
        get { return (bool)GetValue(IsEmptyProperty); }
        set { SetValue(IsEmptyProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsEmpty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsEmptyProperty =
        DependencyProperty.Register("IsEmpty", typeof(bool), typeof(EngineerMainWindow), new PropertyMetadata(false));



    BO.Engineer myEng = new BO.Engineer();

    public EngineerMainWindow(int engineerId = 0)
    {
        myEng = s_bl.Engineer.Read(engineerId);//if an exception is thrown, it will be catched in the EngineerIdWindow.
        if (myEng.Task != null) MyTask = s_bl.Task.Read(myEng.Task.Id);
        else
        {
            TaskList = s_bl.Task.GetTaskList(task => (task.Engineer == null || task.Engineer.Id == engineerId)
                                                     && !(task.Copmlexity > myEng.Level)
                                                     && !s_bl.HasPrevTask(task.Id)
                                                     && task.CompleteDate == null);
            //לא מבוצעות על ידי מהנדס אחרV
            //אין משימות קודמות שלא הסתיימוV
            //אותה רמה או רמה נמוכה יותרV


            /*if (!TaskList.Any())
            {
                MessageBox.Show("There is no available task for this engineer");
                //Close(); return;
            }*/
            if (TaskList.Count() == 0)
            {
                IsEmpty = true;
            }
        }

        InitializeComponent();
    }

    private void FinishTaskButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.FinishTask(myEng.Id, MyTask!.Id);
        MyTask = null;
        TaskList = s_bl.Task.GetTaskList(task => task.Engineer == null
                                                        && !(task.Copmlexity > myEng.Level)
                                                        && !s_bl.HasPrevTask(task.Id)
                                                        && task.CompleteDate == null);
        if (TaskList.Count() == 0)
        {
            IsEmpty = true;
        }
    }
    private void UpdateTaskButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.Task.Update(MyTask!);
        MessageBox.Show("Task Updated Succesfully"); //If we succeeded, we will notify the user.

    }

    private void TaskSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        int id = ((sender as ListView)!.SelectedItem as BO.TaskInList)!.Id;
        MyTask = s_bl.Task.Read(id);
        s_bl.AssignEngineer(myEng.Id, MyTask!.Id);
        s_bl.StartNewTask(myEng.Id, MyTask!.Id);
        MyTask = s_bl.Task.Read(id);//כדי שתאריך ההתחלה יתעדכן

    }
}
