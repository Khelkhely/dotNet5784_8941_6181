namespace BlApi;
/// <summary>
/// 
/// </summary>
public interface ITask
{
    public IEnumerable<BO.Task> ReadAll();
    public BO.Task Read(int id);
    public void Create(BO.Task task);  
    public void Delete(int id);
    public void Update(BO.Task task);
    public void UpdateTaskDate(int id, DateTime date);
}
