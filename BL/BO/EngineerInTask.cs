namespace BO;

/// <summary>
/// information about an engineer that is presented inside a task
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Name">Engineer's full name</param>
public class EngineerInTask
{
    public int Id { get; init; }
    public string? Name { get; set; }

    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
