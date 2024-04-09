using DalApi;
using System.Diagnostics;
namespace Dal;
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public DateTime? StartDate { get => Config.projectStartDate; set => Config.projectStartDate = value;}
    public DateTime? EndDate { get => Config.projectEndDate; set => Config.projectEndDate = value;}

    public void InitializeIds()
    {
        Config.NextTaskId = 1000;
        Config.NextDependencyId = 100;
    }
}