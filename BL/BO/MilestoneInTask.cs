namespace BO;
/// <summary>
/// information about a milestone that is presented inside a task
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Alias">How the milestone is called</param>

public class MilestoneInTask
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
