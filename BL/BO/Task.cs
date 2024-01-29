using System;

namespace BO;

public class Task
{
/*
  CreatedAtDate datetime[not null, note: 'Date when the task was added to the system']
  Status BO.Status[note: 'calculated']
  Dependencies "List<BO.TaskInList>" [ref: > BOTIL.Id, note: 'relevant only before schedule is built']
    Milestone BO.MilestoneInTask [ref: > BOMIT.Id, note: 'calculated when building schedule, populated if there is milestone in dependency, relevant only after schedule is built']
  RequiredEffortTime TimeSpan [note: 'how many men-days needed for the task']
  StartDate datetime [note: 'the real start date']
  ScheduledDate datetime [note: 'the planned start date']
  ForecastDate datetime [note: 'calcualed planned completion date']
  DeadlineDate datetime [note: 'the latest complete date']
  CompleteDate datetime [note: 'real completion date']
  Deliverables string[note: 'description of deliverables for MS copmletion']
  Remarks string[note: 'free remarks from project meetings']
  Engineer BO.EngineerInTask[ref: -BOEIT.Id]
  Copmlexity BO.EngineerExperience[note: 'minimum expirience for engineer to assign']
    
    */
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public status Status { get; set; }
    
}
