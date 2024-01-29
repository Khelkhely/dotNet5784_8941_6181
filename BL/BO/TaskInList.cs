namespace BO;

/// <summary>
/// Information about a task that is presented inside a list
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Alias">how the task is called</param>
/// <param name="Description"></param>
/// <param name="Status"></param>
public class TaskInList
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }

    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
