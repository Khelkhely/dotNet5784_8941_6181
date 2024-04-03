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
                MyTask = (id == 0) ? new BO.Task()
                //if the Id is 0, it means that we want to add a new task, so we will display on the sceen default values.
                    : s_bl.Task.Read(id); //else, we would like to update an existing engineer so we will display his old data.
            }
            catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
