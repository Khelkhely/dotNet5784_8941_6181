﻿using System;
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

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// a flag that signals if the project is already after the scheduling
    /// </summary>
    public bool IsSchedule 
    {
        get { return (bool)GetValue(IsScheduleProperty); }
        set { SetValue(IsScheduleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsSchedule.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsScheduleProperty =
        DependencyProperty.Register("IsSchedule", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(false));

    /// <summary>
    /// the lists of tasks that is displayed 
    /// </summary>
    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
    
    /// <summary>
    /// the EngineerExperience level chosen for the filter
    /// </summary>
    public BO.EngineerExperience MyLevel
    {
        get { return (BO.EngineerExperience)GetValue(MyLevelProperty); }
        set { SetValue(MyLevelProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyLevel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyLevelProperty =
        DependencyProperty.Register("MyLevel", typeof(BO.EngineerExperience), typeof(TaskListWindow), new PropertyMetadata(BO.EngineerExperience.None));


    /// <summary>
    /// the task status that is chosen for the filtering
    /// </summary>
    public BO.Status MyStatus
    {
        get { return (BO.Status)GetValue(MyStatusProperty); }
        set { SetValue(MyStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyStatusProperty =
        DependencyProperty.Register("MyStatus", typeof(BO.Status), typeof(TaskListWindow), new PropertyMetadata(BO.Status.All));



    public TaskListWindow()
    {
        try
        {
            InitializeComponent();
            TaskList = s_bl.Task.GetTaskList();
            IsSchedule = s_bl.IsScheduled();
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// Help Method that updates the TaskList
    /// </summary>
    void UpdateList()
    {
        try
        {
            TaskList = (MyStatus == BO.Status.All) ? s_bl.Task.GetTaskList(x => x.Copmlexity <= MyLevel) :
                        s_bl.Task.GetTaskList(x => x.Status == MyStatus && x.Copmlexity <= MyLevel);
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// TaskList selected event
    /// </summary>
    private void TaskList_TaskSelected(object sender, MouseButtonEventArgs e)
    {
        try
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                new TaskWindow(task.Id).ShowDialog();
                UpdateList();
            }
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// "Add Task" button click event
    /// </summary>
    private void Add_Task_Click(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
        UpdateList();
    }

    /// <summary>
    /// Task filter changed event
    /// </summary>
    private void Task_Filter_Changed(object sender, SelectionChangedEventArgs e)
    {
        UpdateList();
    }
}
