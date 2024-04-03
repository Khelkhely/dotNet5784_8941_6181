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

namespace PL;

/// <summary>
/// Interaction logic for EngineerMainWindow.xaml
/// </summary>
public partial class EngineerMainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    BO.Task myTask;
    BO.Engineer myEng;
    bool hasTask = false;
    bool dosentHaveTask = true;
    public EngineerMainWindow(int id)
    {
        myEng = s_bl.Engineer.Read(id);
        InitializeComponent();

        try
        {
            if (myEng.Task != null)
            {
                hasTask = true;
                dosentHaveTask = false;
                myTask = s_bl.Task.Read(myEng.Task.Id);
            }

        }
        catch (Exception ex)//If an exception is thrown, it will be displayed on the screen in a message box.
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private void FinishTaskButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.finishTask(myEng.Id, myTask.Id);
        ReloadData();
    }

    void ReloadData()
    {
        //maybe not neccery
    }


}
