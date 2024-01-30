using BlApi;
using BO;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;
    public void Create(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task Read(int id)
    {
        DO.Task d = _dal.Task.Read(id) ?? throw new BlDoesNotExistException($"Task with id: {id} doesn't exist");
        BO.Task b = new BO.Task() { Id = id, 
            Alias = d.Alias, 
            Description = d.Description, 
            CreatedAtDate = d.CreatedAtDate, 
            ScheduledDate = d.ScheduledDate, 
            StartDate = d.StartDate, 
            DeadlineDate = d.DeadlineDate, 
            CompleteDate = d.CompleteDate, 
            Deliverables = d.Deliverables, 
            Remarks = d.Remarks, Copmlexity = (BO.EngineerExperience)d.Complexity};
        b.Status = CalculateStatus(b);
        //TODO: Dependencies, Milestone, ForecastDate, RequiredEffortTime, Engineer
        return b;
    }

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void UpdateTaskDate(int id, DateTime date)
    {
        throw new NotImplementedException();
    }

    public Status CalculateStatus (BO.Task b)
    {
        if (b.ScheduledDate == null)
            return Status.Unscheduled;
        if (b.StartDate == null)
            return Status.Scheduled;
        if (b.CompleteDate == null)
            return Status.OnTrack;
        if (b.CompleteDate < DateTime.Today)
            return Status.Done;
        return b.Status = Status.InJeopardy;
    }
}
