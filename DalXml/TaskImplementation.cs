namespace Dal;
using DalApi;
using DO;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    public void Clear()
    {
        XMLTools.SaveListToXMLSerializer<Task>(new List<Task>(), s_tasks_xml);
    }
    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Task newTask = item with { Id = Config.NextTaskId };
        tasks.Add(newTask);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
        return newTask.Id;
    }

    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if(tasks.RemoveAll(x => x.Id == id) == 0)
            throw new DalDoesNotExistException($"Task with ID={id} doesn't exist");
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }

    public Task? Read(int id)
    {
        IEnumerable<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);

        Task? result = tasks.FirstOrDefault(x => x.Id == id);

        return result;
    }

    public Task? Read(Func<Task, bool> filter)
    {
        IEnumerable<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);

        Task? result = tasks.FirstOrDefault(x => filter(x));

        return result;
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (filter == null)
            return tasks;
        return tasks.Where(filter);
    }

    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (tasks.RemoveAll(x => x.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Task with ID={item.Id} doesn't exist");

        tasks.Add(item);

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

    }
}