using DalApi;
namespace Dal;

sealed public class DalXml
{
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImlementation();
    public IDependency Dependency => new DependencyImplementation();
}
