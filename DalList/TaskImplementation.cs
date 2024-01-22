namespace Dal;
using DalApi;
using DO;

internal class TaskImplementation : ITask
{
    public void Clear()
    {
        DataSource.Tasks.Clear();
    }
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
            throw new DalDoesNotExistException($"Task with ID={id} doesn't exist");
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(item => item.Id == id);
    }
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }

    public void Update(Task item)
    {
        if (DataSource.Tasks.Exists(x => x.Id == item.Id))
        {
            DataSource.Tasks.RemoveAll(x => x.Id == item.Id);
            DataSource.Tasks.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Task with ID={item.Id} doesn't exist");
    }
}
