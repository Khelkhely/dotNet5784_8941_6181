namespace Dal;
using DalApi;
using DO;

internal class DependencyImplementation : IDependency
{
    public void Clear()
    {
        DataSource.Dependencies.Clear();
    }

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
            throw new DalDoesNotExistException($"Dependency with ID={id} doesn't exist");
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(item => item.Id == id);
    }
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
    }


    public void Update(Dependency item)
    {
        if (DataSource.Dependencies.Exists(x => x.Id == item.Id))
        {
            DataSource.Dependencies.RemoveAll(x => x.Id == item.Id);
            DataSource.Dependencies.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doesn't exist");
    }
}
