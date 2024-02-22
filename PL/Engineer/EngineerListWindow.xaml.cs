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

    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EngineerList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }

    private void Engineer_Filter_Changed(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (level == BO.EngineerExperience.None) ? s_bl.Engineer.ReadAll() :
            s_bl.Engineer.ReadAll(x => x.Level == level);
    }

    private void Add_Engineer_Click(object sender, RoutedEventArgs e)
    {
        new EngineerWindow().ShowDialog();
        EngineerList = (level == BO.EngineerExperience.None) ? s_bl.Engineer.ReadAll() :
            s_bl.Engineer.ReadAll(x => x.Level == level);
    }

    private void EngineerList_DoubleClick(object sender, MouseButtonEventArgs e)
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
