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

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public bool Add = true;


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
            Add = id == 0; // add is true if id is 0.
            MyEngineer = (id == 0) ? new BO.Engineer() { Id = 0, Cost = 0, Level = BO.EngineerExperience.None }
            //if the Id is 0, it means that we want to add a new engineer, so we will display on the sceen default values.
                : s_bl.Engineer.Read(id); //else, we would like to update an existing engineer so we will display his old data.
        }
        catch(Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }

    /// <summary>
    /// "Add/Update" button click event
    /// </summary>
    private void AddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Close();
            if (Add) //if we want to add a new engineer
            {
                s_bl.Engineer.Create(MyEngineer);
                MessageBox.Show("Engineer Added Succesfully"); //If we succeeded, we will notify the user.
            }
            else //if we want to update an existing engineer
            {
                s_bl.Engineer.Update(MyEngineer);
                MessageBox.Show("Engineer Updated Succesfully"); //If we succeeded, we will notify the user.
            }
        }
        catch(Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Engineer.Delete(MyEngineer.Id);
            Close();
        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}