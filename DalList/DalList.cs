namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public ITask Task => new TaskImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public IEngineer Engineer => new EngineerImplementation();
}
