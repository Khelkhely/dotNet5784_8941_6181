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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public bool IsSchedule
        {
            get { return (bool)GetValue(IsScheduleProperty); }
            set { SetValue(IsScheduleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSchedule.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsScheduleProperty =
            DependencyProperty.Register("IsSchedule", typeof(bool), typeof(TaskWindow), new PropertyMetadata(false));
        
        public bool TaskSelected
        {
            get { return (bool)GetValue(TaskSelectedProperty); }
            set { SetValue(TaskSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskSelectedProperty =
            DependencyProperty.Register("TaskSelected", typeof(bool), typeof(TaskWindow), new PropertyMetadata(false));

        public BO.TaskInList MyDependency
        {
            get { return (BO.TaskInList)GetValue(MyDependencyProperty); }
            set { SetValue(MyDependencyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyDependency.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyDependencyProperty =
            DependencyProperty.Register("MyDependency", typeof(BO.TaskInList), typeof(TaskWindow), new PropertyMetadata(null));

        public List<BO.TaskInList> NewDepList
        {
            get { return (List<BO.TaskInList>)GetValue(NewDepListProperty); }
            set { SetValue(NewDepListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewDepList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewDepListProperty =
            DependencyProperty.Register("NewDepList", typeof(List<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

        public BO.EngineerInTask MyEngineer
        {
            get { return (BO.EngineerInTask)GetValue(MyEngineerProperty); }
            set { SetValue(MyEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyEngineerProperty =
            DependencyProperty.Register("MyEngineer", typeof(BO.EngineerInTask), typeof(TaskWindow), new PropertyMetadata(null));

        public BO.Task MyTask
        {
            get { return (BO.Task)GetValue(MyTaskProperty); }
            set { SetValue(MyTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyTaskProperty =
            DependencyProperty.Register("MyTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));


        public TaskWindow(int id = 0)
        {
            try
            {
                IsSchedule = s_bl.IsScheduled();
                InitializeComponent();
                MyTask = (id == 0) ? new BO.Task() { Id = 0, CreatedAtDate = s_bl.Clock }
                //if the Id is 0, it means that we want to add a new task, so we will display on the sceen default values.
                    : s_bl.Task.Read(id); //else, we would like to update an existing task so we will display its old data.
                NewDepList = MyTask.Dependencies ?? new List<BO.TaskInList> { };
                MyEngineer = MyTask.Engineer!;
            }
            catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// "Add/Update" button click event
        /// </summary>
        private void Add_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MyTask.Id == 0) //if we want to add a new Task
                {
                    s_bl.Task.Create(MyTask);
                    MessageBox.Show("Task Added Succesfully"); //If we succeeded, we will notify the user.
                }
                else //if we want to update an existing Task
                {
                    s_bl.Task.Update(MyTask);
                    MessageBox.Show("Task Updated Succesfully"); //If we succeeded, we will notify the user.
                }
                Close();
            }
            catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// "Add dependency" button click event
        /// </summary>
        private void AddDependency(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Task t = MyTask;
                new AddDependencyWindow(ref t).ShowDialog();
                if (MyTask.Dependencies != null)
                    NewDepList = (from dep in MyTask.Dependencies
                                  orderby dep.Id
                                  select dep).ToList();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// "Delete" button click event
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete the task?",
                    "Delete task", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    s_bl.Task.Delete(MyTask.Id);
                    Close();
                }
            }
            catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// "Choose engineer" button click event
        /// </summary>
        private void ChooseEngineer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Task t = MyTask;
                new EngineerListToAssignWindow(ref t).ShowDialog();
                MyEngineer = MyTask.Engineer!;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// "Remove dependency" button click event
        /// </summary>
        private void RemoveDep_Click(object sender, RoutedEventArgs e)
        {
            if (MyDependency != null)
            {
                MyTask.Dependencies?.RemoveAll(x => x.Id == MyDependency.Id);
                NewDepList = (from dep in MyTask.Dependencies
                              select dep).ToList();
                TaskSelected = false;
                MyDependency = null;
            }
        }

        /// <summary>
        /// Dependency selected event
        /// </summary>
        private void Dependency_Selected(object sender, MouseButtonEventArgs e)
        {
            TaskSelected = true;
            MyDependency = (sender as ListView)?.SelectedItem as BO.TaskInList;
        }
    }
}

