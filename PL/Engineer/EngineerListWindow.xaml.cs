using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.EngineerExperience level { get; set; } = BO.EngineerExperience.None;

    /// <summary>
    /// the list of engineers that will be displayed on the screen
    /// </summary>
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EngineerList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    /// <summary>
    /// constructor
    /// </summary>
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!; //initialize the list of engineers
    }

    /// <summary>
    /// reloads the list according to the engineer experience level chosen in the combobox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Engineer_Filter_Changed(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (level == BO.EngineerExperience.None) ? s_bl.Engineer.ReadAll() :
            s_bl.Engineer.ReadAll(x => x.Level == level); //change the list according to the chosen filter
    }

    /// <summary>
    /// opens the engineer window in adding mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Add_Engineer_Click(object sender, RoutedEventArgs e)
    {
        new EngineerWindow().ShowDialog();
        EngineerList = (level == BO.EngineerExperience.None) ? s_bl.Engineer.ReadAll() :
            s_bl.Engineer.ReadAll(x => x.Level == level); //reloads the list with the new update
    }

    /// <summary>
    /// opens an engineer window in update mode for the engineer that was chosen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EngineerList_SelectedEngineer(object sender, MouseButtonEventArgs e)
    {
        BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
        if (engineer != null)
        {
            new EngineerWindow(engineer.Id).ShowDialog();
            EngineerList = (level == BO.EngineerExperience.None) ? s_bl.Engineer.ReadAll() :
            s_bl.Engineer.ReadAll(x => x.Level == level);

        }
    }
}
