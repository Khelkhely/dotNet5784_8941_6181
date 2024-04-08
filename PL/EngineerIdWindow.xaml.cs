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

namespace PL;

/// <summary>
/// Interaction logic for EngineerIdWindow.xaml
/// </summary>
public partial class EngineerIdWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public int MyId
    {
        get { return (int)GetValue(MyIdProperty); }
        set { SetValue(MyIdProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyId.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyIdProperty =
        DependencyProperty.Register("MyId", typeof(int), typeof(EngineerIdWindow), new PropertyMetadata(0));

    public EngineerIdWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// "OK" button click event
    /// </summary>
    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            new EngineerMainWindow(MyId).Show();

        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
