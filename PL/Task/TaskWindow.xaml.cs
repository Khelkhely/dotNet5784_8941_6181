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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //private bool flag = false;

        public Visibility DependenciesToAdd
        {
            get { return (Visibility)GetValue(DependenciesToAddProperty); }
            set { SetValue(DependenciesToAddProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DependenciesToAdd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DependenciesToAddProperty =
            DependencyProperty.Register("DependenciesToAdd", typeof(Visibility), typeof(TaskWindow), new PropertyMetadata(Visibility.Hidden));

        public bool IsSchedule
        {
            get { return (bool)GetValue(IsScheduleProperty); }
            set { SetValue(IsScheduleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSchedule.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsScheduleProperty =
            DependencyProperty.Register("IsSchedule", typeof(bool), typeof(TaskWindow), new PropertyMetadata(false));



        public Visibility DependenciesListView
        {
            get { return (Visibility)GetValue(DependenciesListViewProperty); }
            set { SetValue(DependenciesListViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DependenciesListView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DependenciesListViewProperty =
            DependencyProperty.Register("DependenciesListView", typeof(Visibility), typeof(TaskWindow), new PropertyMetadata(Visibility.Visible));




        public List<BO.TaskInList> TaskList
        {
            get { return (List<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(List<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

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
            if (s_bl.GetStartDate() != null)
                IsSchedule = true;
            InitializeComponent();
            try
            {
                //add = id == 0; // add = true if id is 0.
                MyTask = (id == 0) ? new BO.Task() { Id = 0, CreatedAtDate = s_bl.Clock }
                //if the Id is 0, it means that we want to add a new task, so we will display on the sceen default values.
                    : s_bl.Task.Read(id); //else, we would like to update an existing engineer so we will display his old data.
            }
            catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
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
            }
            catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddDependency(object sender, RoutedEventArgs e)
        {
            if (DependenciesListView == Visibility.Visible)
            {
                TaskList = s_bl.Task.GetTaskList(x => (x.Id != MyTask.Id)
                                             && (!s_bl.Task.Read(x.Id).Dependencies?.Any(t => t.Id == MyTask.Id) ?? true))//shows only the tasks that don't depends on this task already
                                                .ToList();
                //remove all the tasks that MyTask is already dependant on
                if (MyTask.Dependencies != null)
                    foreach (var task in MyTask.Dependencies)
                    {
                        TaskList.RemoveAll(x => x.Id == task.Id);
                    }

                DependenciesToAdd = Visibility.Visible;
                DependenciesListView = Visibility.Hidden;
            }
            else
            {
                //return to view the dependencies
                DependenciesToAdd = Visibility.Hidden;
                DependenciesListView = Visibility.Visible;
            }
        }

        private void ChooseTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                if (MyTask.Dependencies != null) //add the task to the dependencies               
                    MyTask.Dependencies.Add(task);
                else
                    MyTask.Dependencies = new List<BO.TaskInList> { task };

                //return to view the dependencies
                DependenciesToAdd = Visibility.Hidden;
                DependenciesListView = Visibility.Visible;

                
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.Delete(MyTask.Id);
                Close();
            }
            catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChooseEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListToAssignWindow(MyTask.Id).Show();
            Close();
        }
    }


    //

}

