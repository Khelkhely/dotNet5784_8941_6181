namespace BO;

/// <summary>
/// Information about a milestone in the project
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Alias">how the milestone is called</param>
/// <param name="Description">a description of the milestone</param>
/// <param name="CreatedAtDate">Date when the task was added to the system</param>
/// <param name="Status">calculated</param>
/// <param name="ForecastDate">a revised scheduled completion date</param>
/// <param name="DeadlineDate">the latest complete date</param>
/// <param name="CompleteDate">real completion date</param>
/// <param name="CompletionPercentage">percentage of completed tasks - calculated</param>
/// <param name="Remarks">free remarks from project meetings</param>
/// <param name="Dependencies">list of tasks that the milestone is dependant on</param>
public class Milestone
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAtDate { get; init; }
    public Status Status { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public double? CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<TaskInList>? Dependencies { get; set; }

    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
