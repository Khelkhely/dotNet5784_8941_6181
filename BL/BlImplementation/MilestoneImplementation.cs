using BlApi;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = Factory.Get;
    public void CreateSchedule()
    {
        throw new NotImplementedException();
    }

    public BO.Milestone Read(int id)
    {
        DO.Task? doMilestone = _dal.Task.Read(id);
        if (doMilestone == null || !doMilestone.IsMilestone) 
            throw new BO.BlDoesNotExistException($"Milestone with id: {id} doesn't exist");
        BO.Milestone boMs = new BO.Milestone()
        {
            Id = id,
            Alias = doMilestone.Alias,
            Description = doMilestone.Description,
            CreatedAtDate = doMilestone.CreatedAtDate,
            DeadlineDate = doMilestone.DeadlineDate,
            CompleteDate = doMilestone.CompleteDate,
            Remarks = doMilestone.Remarks
        };
        IEnumerable<DO.Task> previous = from item in _dal.Dependency.ReadAll(x => x.DependentTask == id)//the dependencies of the milestone
                                        let  x = _dal.Task.Read(item.DependsOnTask) ?? throw new BO.BlDoesNotExistException($"task with id: {item.DependsOnTask} doesn't exist")
                                        select x; //get a collection of all the previous tasks (dal)

        //make a list of the previous tasks as BO.TaskInList
        boMs.Dependencies = (from task in previous 
                          select new BO.TaskInList
                          {
                              Id = task.Id,
                              Alias = task.Alias,
                              Description = task.Description,
                              Status = BO.Tools.CalculateStatus(task)

                          }).ToList();

        //calculate percentage of the tasks before the milestone that are done
        boMs.CompletionPercentage = (boMs.Dependencies.Count(x => x.Status == BO.Status.Done) / boMs.Dependencies.Count()) * 100;

        //calculate the status of the milestone
        boMs.Status = boMs.Dependencies.Min(x => x.Status);

        //the milestone's forcast date is the latest forcast date of all the previous tasks.
        //can't calculate the milestones' forcast date if the previous tasks don't have one yet
        if (boMs.Status == BO.Status.Unscheduled)
            boMs.ForecastDate = null;
        else
            boMs.ForecastDate = previous.Max(BO.Tools.CalculateForcast);
        return boMs;
    }

    public BO.Milestone Update(int id)
    {
        DO.Task? doMilestone = _dal.Task.Read(id);
        if (doMilestone == null || !doMilestone.IsMilestone)
            throw new BO.BlDoesNotExistException($"Milestone with id: {id} doesn't exist");
        Console.WriteLine("Enter the milestone's new alias:\t");
        string? alias = Console.ReadLine();
        if (alias == "") alias = doMilestone.Alias;
        Console.WriteLine("Enter the milestone's new description:\t");
        string? description = Console.ReadLine();
        if (description == "") description = doMilestone.Description;
        Console.WriteLine("Enter the milestone's new remarks:\t");
        string? remarks = Console.ReadLine();
        if(remarks == "") remarks = doMilestone.Remarks;
        DO.Task newMilestone = doMilestone with { Alias = alias, Description = description, Remarks = remarks };
        try
        {
            _dal.Task.Update(newMilestone);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Milestone with id: {id} doesn't exist", ex);
        }
        return Read(id);
    }
}
