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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        static bool add;

        public BO.Engineer MyEngineer
        {
            get { return (BO.Engineer)GetValue(MyEngineerProperty); }
            set { SetValue(MyEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyEngineerProperty =
            DependencyProperty.Register("MyEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        public EngineerWindow(int id = 0)
        {
            InitializeComponent();
            try
            {
                add = id == 0;
                MyEngineer = (id == 0) ? new BO.Engineer() { Id = 0, Cost = 0, Level = BO.EngineerExperience.None }
                    : s_bl.Engineer.Read(id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void AddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (add)
                {
                    s_bl.Engineer.Create(MyEngineer);
                    MessageBox.Show("Engineer Added Succesfully");
                }
                else
                {
                    s_bl.Engineer.Update(MyEngineer);
                    MessageBox.Show("Engineer Updated Succesfully");
                }
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
    }
}
