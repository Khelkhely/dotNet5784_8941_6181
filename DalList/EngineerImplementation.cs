namespace Dal;
using DalApi;
using DO;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Exists(x => x.Id == item.Id))
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (DataSource.Engineers.Exists(x => x.Id == id))
            DataSource.Engineers.RemoveAll(x => x.Id == id);
        else
            throw new DalDoesNotExistException($"Engineer with ID={id} doesn't exist");
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item.Id == id);
    }

    public Engineer? Read(Func<Engineer,bool> filter) 
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        if (DataSource.Engineers.Exists(x => x.Id == item.Id))
        {
            DataSource.Engineers.RemoveAll(x => x.Id == item.Id);
            DataSource.Engineers.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} doesn't exist");
    }
}
