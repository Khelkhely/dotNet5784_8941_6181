namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public ITask Task => new TaskImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public IEngineer Engineer => new EngineerImplementation();

    public DateTime? StartDate
    {
        get => DataSource.Config.projectStartDate; 
        set => DataSource.Config.projectStartDate = value;
    }
    public DateTime? EndDate
    {
        get => DataSource.Config.projectEndDate;
        set => DataSource.Config.projectEndDate = value;
    }

    public void InitializeIds()
    {
        DataSource.Config.nextDependencyId = 100;
        DataSource.Config.nextTaskId = 1000;
    }
}
