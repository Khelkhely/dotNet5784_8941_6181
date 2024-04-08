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

    public bool IsEmpty//if TaskList is empty
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
        try
        {
            if (myEng.Task != null) MyTask = s_bl.Task.Read(myEng.Task.Id);
            else
            {
                TaskList = s_bl.Task.GetTaskList(task => (task.Engineer == null || task.Engineer.Id == engineerId)//does not belong to enother engineer
                                                         && !(task.Copmlexity > myEng.Level)//the level of the engineer is not smaller then the complexity
                                                         && !s_bl.HasPrevTask(task.Id)//there is not previouse tasks that didn't complete
                                                         && task.CompleteDate == null);

                if (TaskList.Count() == 0)
                {
                    IsEmpty = true;
                }
            }

            InitializeComponent();
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// "Finish Task" button click event
    /// </summary>
    private void FinishTaskButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.FinishTask(myEng.Id, MyTask!.Id);
            MyTask = null;
            TaskList = s_bl.Task.GetTaskList(task => (task.Engineer == null || task.Engineer.Id == myEng.Id)//does not belong to enother engineer
                                                            && !(task.Copmlexity > myEng.Level)//the level of the engineer is not smaller then the complexity
                                                            && !s_bl.HasPrevTask(task.Id)//there is not previouse tasks that didn't complete
                                                            && task.CompleteDate == null);
            if (TaskList.Count() == 0)
            {
                IsEmpty = true;
            }
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// "Update" button click event
    /// </summary>
    private void UpdateTaskButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Task.Update(MyTask!);
            MessageBox.Show("Task Updated Succesfully"); //If we succeeded, we will notify the user.

        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// Task Selected event
    /// </summary>
    private void TaskSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            int id = ((sender as ListView)!.SelectedItem as BO.TaskInList)!.Id;
            MyTask = s_bl.Task.Read(id);
            s_bl.AssignEngineer(myEng.Id, MyTask!.Id);
            s_bl.StartNewTask(myEng.Id, MyTask!.Id);
            MyTask = s_bl.Task.Read(id);//update the start date
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
