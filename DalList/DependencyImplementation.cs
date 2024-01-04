namespace Dal;
using DalApi;
using DO;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        Dependency newItem = item with { Id = DataSource.Config.NextDependencyId };
        DataSource.Dependencies.Add(newItem);
        return newItem.Id;
    }

    public void Delete(int id)
    {
        if (DataSource.Dependencies.Exists(x => x.Id == id))
            DataSource.Dependencies.RemoveAll(x => x.Id == id);
        else
            throw new Exception($"Dependency with ID={id} doesn't exist");
    }

    public Dependency? Read(int id)
    {
        if (DataSource.Dependencies.Exists(x => x.Id == id))
            return DataSource.Dependencies.Find(x => x.Id == id);
        return null;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        if (DataSource.Dependencies.Exists(x => x.Id == item.Id))
        {
            DataSource.Dependencies.RemoveAll(x => x.Id == item.Id);
            DataSource.Dependencies.Add(item);
        }
        else
            throw new Exception($"Dependency with ID={item.Id} doesn't exist");
    }
}
