namespace DO;
/// <summary>
/// tasks and information about them in the project
/// </summary>
/// <param name="Id">automatic identification number</param>
/// <param name="Alias">how the task is called</param>
/// <param name="Description">description of the task</param>
/// <param name="IsMilestone">is the task a milestone in the project</param>
/// <param name="CreatedAtDate">Date when the task was added to the system</param>
/// <param name="ScheduledDate">planned date to start the task</param>
/// <param name="StartDate">actual starting date</param>
/// <param name="RequiredEffortTime">number of work days needed for the task</param>
/// <param name="DeadlineDate">last possible completion date</param>
/// <param name="CompleteDate">actual completion date</param>
/// <param name="Deliverables">product of the task</param>
/// <param name="Remarks">free remarks from project meetings</param>
/// <param name="EngineerId">Id number of the engineer given the task</param>
/// <param name="Complexity">minimum expirience for engineer to assign task to</param>
public record Task
(
    int Id,
    string? Alias = null,
    string? Description = null,
    bool IsMilestone = false,
    DateTime? CreatedAtDate = null,
    DateTime? ScheduledDate = null,
    DateTime? StartDate = null,
    TimeSpan? RequiredEffortTime = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int EngineerId = 0,
    DO.EngineerExperience Complexity = EngineerExperience.Beginner
)
{
    public Task() : this(0) { } //empty ctor
}
