namespace BO;
/// <summary>
/// task and information about it in the project
/// </summary>
/// <param name="Id">Unique Id number</param>
/// <param name="Alias">How the task is called</param>
/// <param name="Description">Description of the task</param>
/// <param name="Status">The status of the task (calculated)</param>
/// <param name="Dependencies">List of dependencies (relevant only before schedule is built)</param>
/// <param name="Milestone">Related milestone (id and alias) if exists (otherwise null)</param>
/// <param name="CreatedAtDate">Date when the task was added to the system</param>
/// <param name="ScheduledDate">Planned date to start the task</param>
/// <param name="StartDate">Actual starting date</param>
/// <param name="ForecastDate">Calcualed planned completion date</param>
/// <param name="DeadlineDate">Last possible completion date</param>
/// <param name="CompleteDate">Actual completion date</param>
/// <param name="RequiredEffortTime">Number of work days needed for the task</param>
/// <param name="Deliverables">Description of deliverables for MS copmletion</param>
/// <param name="Remarks">Free remarks from project meetings</param>
/// <param name="Engineer">If exists, the ID and name of the engineer assigned to the task</param>
/// <param name="Copmlexity">Minimum expirience for engineer to assign</param>

public class Task
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public List<TaskInList>? Dependencies { get; set; }
    public MilestoneInTask? Milestone { get; set; }
    public DateTime? CreatedAtDate { get; init; }//צריך להיות נאל? כי לא נוכל לשנות בהמשך
    public DateTime? StartDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience Copmlexity  { get; set;}
    public override string ToString()
    {
        return this.ToStringProperty();//!לתקן את הפונקציה לאוספים
    }

}
