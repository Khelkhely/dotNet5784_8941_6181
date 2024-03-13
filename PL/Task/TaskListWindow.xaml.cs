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
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl.Task.GetTaskList();
        }

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));



        public BO.EngineerExperience MyLevel
        {
            get { return (BO.EngineerExperience)GetValue(MyLevelProperty); }
            set { SetValue(MyLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyLevelProperty =
            DependencyProperty.Register("MyLevel", typeof(BO.EngineerExperience), typeof(TaskListWindow), new PropertyMetadata(BO.EngineerExperience.None));



        void UpdateList()
        {
            TaskList = (MyLevel == BO.EngineerExperience.None) ? s_bl.Task.GetTaskList() : 
                s_bl.Task.GetTaskList(x => x.Copmlexity == MyLevel);
        }

        private void TaskList_TaskSelected(object sender, MouseButtonEventArgs e)
        {

        }

        private void Add_Task_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Task_Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }
    }
}
