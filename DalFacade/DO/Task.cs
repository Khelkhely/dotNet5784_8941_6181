namespace DO;
/// <summary>
/// task entity represents a task and information about it
/// </summary>
/// <param name="Id">
/// <param name="Alias">unique id of task</param>
/// <param name="Description"></param>
/// <param name="CreatedAtDate"></param>
/// <param name="RequiredEffortTime"></param>
/// ...
public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    TimeSpan RequiredEffortTime,
    bool IsMilestone,
    DO.EngineerExperience Complexity,
    DateTime StartDate,
    DateTime ScheduledDate,
    DateTime DeadlineDate,
    DateTime CompleteDate,
    string Deliverables,
    string Remarks,
    int EngineerId
)
{
    
}
