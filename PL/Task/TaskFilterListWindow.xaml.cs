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
    /// Interaction logic for TaskFilterListWindow.xaml
    /// </summary>
    public partial class TaskFilterListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.TaskInList> MyList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(MyListProperty); }
            set { SetValue(MyListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyListProperty =
            DependencyProperty.Register("MyList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskFilterListWindow), new PropertyMetadata(null));


        public TaskFilterListWindow(ref BO.TaskInList chosentask, Func<BO.Task, bool>? filter = null)
        {
            MyList = s_bl.Task.GetTaskList(filter);
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                 
            }
        }
    }
}
