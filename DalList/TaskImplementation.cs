namespace Dal;
using DalApi;
using DO;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        Task newItem = item with { Id = DataSource.Config.NextTaskId };
        DataSource.Tasks.Add(newItem);
        return newItem.Id;
    }

    public void Delete(int id)
    {
        if (DataSource.Tasks.Exists(x => x.Id == id))
            DataSource.Tasks.RemoveAll(x => x.Id == id);
        else
            throw new Exception($"Task with ID={id} doesn't exist");
    }

    public Task? Read(int id)
    {
        if(DataSource.Tasks.Exists(x => x.Id == id))
            return DataSource.Tasks.Find(x => x.Id == id);
        return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (DataSource.Tasks.Exists(x => x.Id == item.Id))
        {
            DataSource.Tasks.RemoveAll(x => x.Id == item.Id);
            DataSource.Tasks.Add(item);
        }
        else
            throw new Exception($"Task with ID={item.Id} doesn't exist");
    }
}
