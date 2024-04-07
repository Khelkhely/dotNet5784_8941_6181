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
    /// Interaction logic for GanttChartWindow.xaml
    /// </summary>
    public partial class GanttChartWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public IEnumerable<BO.Task> TaskList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(GanttChartWindow), new PropertyMetadata(null));


        public GanttChartWindow()
        {
            TaskList = s_bl.Task.ReadAll();
            InitializeComponent();
        }
    }
}
