namespace BO;

/// <summary>
/// Information about an engineer
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Name">Engineer's full name</param>
/// <param name="Email">The email of the engineer</param>
/// <param name="Level">Engineer level</param>
/// <param name="Cost">Cost of the engineer per hour</param>
/// <param name="Task">Id and Alias of the Task that the Engineer is currently working on</param>
public class Engineer
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double? Cost {  get; set; }
    public TaskInEngineer? Task {  get; set; }

    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
