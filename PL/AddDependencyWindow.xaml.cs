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

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDependencyWindow.xaml
    /// </summary>
    public partial class AddDependencyWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        BO.Task? MyTask = null;
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(AddDependencyWindow), new PropertyMetadata(null));

        public AddDependencyWindow(ref BO.Task task)
        {
            InitializeComponent();
            MyTask = task;
            TaskList = s_bl.Task.GetTaskList(x => (x.Id != MyTask.Id) && //not including MyTask itself
                (!s_bl.Task.Read(x.Id).Dependencies?.Any(t => t.Id == MyTask.Id) ?? true) //tasks that don't depends on this task already
                && (!MyTask.Dependencies?.Any(t => t.Id == x.Id) ?? true)); //tasks that MyTask doesn't already depend on
        }

        private void DependendySelected_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? chosenTask = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (chosenTask != null)
            {
                if (MyTask!.Dependencies != null)
                    MyTask!.Dependencies.Add(chosenTask);
                else
                    MyTask!.Dependencies = new List<BO.TaskInList> { chosenTask };
                Close();
            }
        }
    }
}
