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




        public int Length
        {
            get { return (int)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Length.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register("Length", typeof(int), typeof(GanttChartWindow), new PropertyMetadata(0));





        public IEnumerable<int> MyDates
        {
            get { return (IEnumerable<int>)GetValue(MyDatesProperty); }
            set { SetValue(MyDatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyDates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyDatesProperty =
            DependencyProperty.Register("MyDates", typeof(IEnumerable<int>), typeof(GanttChartWindow), new PropertyMetadata(null));




        public GanttChartWindow()
        {
            InitializeComponent();
            DateTime startDate = (DateTime)s_bl.GetStartDate()!;
            DateTime endDate = (DateTime)s_bl.GetEndDate()!;
            Length = (startDate - startDate).Days * 30;
            List<DateTime> dates = new List<DateTime> { };
            for (DateTime d = startDate; d < endDate; d = d.AddDays(1)) 
                dates.Add(d);
            TaskList = s_bl.Task.ReadAll();
            MyDates = from dt in dates
                      select dt.Day;

        }
    }
}
