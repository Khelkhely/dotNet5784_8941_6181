namespace Dal;
using DalApi;
using DO;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Exists(x => x.Id == item.Id))
            throw new Exception($"Student with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (DataSource.Engineers.Exists(x => x.Id == id))
            DataSource.Engineers.RemoveAll(x => x.Id == id);
        else
            throw new Exception($"Engineer with ID={id} doesn't exist");
    }

    public Engineer? Read(int id)
    {
        if (DataSource.Engineers.Exists(x => x.Id == id))
            return DataSource.Engineers.Find(x => x.Id == id);
        return null;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        if (DataSource.Engineers.Exists(x => x.Id == item.Id))
        {
            DataSource.Engineers.RemoveAll(x => x.Id == item.Id);
            DataSource.Engineers.Add(item);
        }
        else
            throw new Exception($"Engineer with ID={item.Id} doesn't exist");
    }
}
