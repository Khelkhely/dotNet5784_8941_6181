namespace BO;

/// <summary>
/// Information about a task that is presented inside an Engineer
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Alias">how the task is called</param>
public class TaskInEngineer
{
    public int Id { get; init; }
    public string? Alias { get; set; }

    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
