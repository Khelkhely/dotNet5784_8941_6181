using System;

namespace BO;
/// <summary>
/// task and information about it in the project
/// </summary>
/// <param name="Id">unique Id number</param>
/// <param name="Alias">how the task is called</param>
/// <param name="Description">description of the task</param>
/// <param name="Status">the status of the task (calculated)</param>
/// <param name="Dependencies">list of dependencies (relevant only before schedule is built)</param>
/// <param name="Milestone">related milestone (id and alias) if exists (otherwise null)</param>
/// <param name="CreatedAtDate">Date when the task was added to the system</param>
/// <param name="ScheduledDate">planned date to start the task</param>
/// <param name="StartDate">actual starting date</param>
/// <param name="ForecastDate">calcualed planned completion date</param>
/// <param name="DeadlineDate">last possible completion date</param>
/// <param name="CompleteDate">actual completion date</param>
/// <param name="RequiredEffortTime">number of work days needed for the task</param>
/// <param name="Deliverables">description of deliverables for MS copmletion</param>
/// <param name="Remarks">free remarks from project meetings</param>
/// <param name="Engineer">if exists, the ID and name of the engineer assigned to the task</param>
/// <param name="Copmlexity">minimum expirience for engineer to assign</param>

public class Task
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public List<TaskInList> Dependencies { get; set; }
    public MilestoneInTask? Milestone { get; set; }
    public DateTime CreatedAtDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime ForecastDate { get; set; }
    public DateTime DeadlineDate { get; set; }
    public DateTime CompleteDate { get; set; }
    public TimeSpan RequiredEffortTime { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask Engineer { get; set; }
    public EngineerExperience Copmlexity  { get; set;}

}
