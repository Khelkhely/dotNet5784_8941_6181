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
using PL;

namespace PL
{
    /// <summary>
    /// Interaction logic for EngineerIdWindow.xaml
    /// </summary>
    public partial class EngineerIdWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        int myId = 0;
        public EngineerIdWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new EngineerMainWindow(myId).Show();

            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
