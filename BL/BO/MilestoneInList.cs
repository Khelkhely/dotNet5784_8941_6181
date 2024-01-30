namespace BO;
/// <summary>
/// information about a milestone that is presented inside a list
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Description">Description of the milestone</param>
/// <param name="Alias">How the milestone is called</param>
/// <param name="Status">The status of the milestone</param>
/// <param name="CompletionPercentage">Percentage of completed tasks (calculated)</param>

public class MilestoneInList
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public Status Status { get; set; }
    public double CompletionPercentage { get; set; }
}
