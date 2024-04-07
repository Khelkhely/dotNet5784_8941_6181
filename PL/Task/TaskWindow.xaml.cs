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
        private bool flag = false;

        //public Visibility Vs { get; set; } = Visibility.Collapsed;



        public Visibility Vs
        {
            get { return (Visibility)GetValue(VsProperty); }
            set { SetValue(VsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VsProperty =
            DependencyProperty.Register("Vs", typeof(Visibility), typeof(TaskWindow), new PropertyMetadata(Visibility.Hidden));




        public Visibility Vd
        {
            get { return (Visibility)GetValue(VdProperty); }
            set { SetValue(VdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VdProperty =
            DependencyProperty.Register("Vd", typeof(Visibility), typeof(TaskWindow), new PropertyMetadata(Visibility.Visible));




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
            TaskList = s_bl.Task.GetTaskList().ToList();
            if (MyTask.Dependencies != null)
                foreach (var task in MyTask.Dependencies)
                {
                    TaskList.RemoveAll(x => x.Id == task.Id);
                }
            Vs = Visibility.Visible;
            Vd = Visibility.Hidden;
            /*BO.TaskInList newTask = new BO.TaskInList();
            new TaskFilterListWindow(ref newTask, availabe).ShowDialog();
            if (MyTask.Dependencies != null)
                MyTask.Dependencies.Add(newTask);
            else
                MyTask.Dependencies = new List<BO.TaskInList> { newTask };*/

        }

        /*private bool availabe(BO.Task task)
        {
            if (MyTask.Dependencies != null)
            {
                if (MyTask.Dependencies.Count(x => x.Id == task.Id) > 0) //already is in the dependencies 
                    return false;
            }
            return true;
        }*/
    }
}
