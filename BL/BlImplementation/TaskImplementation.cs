using BlApi;
using BO;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;
    public void Create(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BlInvalidInputException("Id isn't a positive number");
        if (task.Alias == "")
            throw new BlInvalidInputException("Alias can't be empty");
        try
        {
            _dal.Task.Create(BoToDo(task));
            if(task.Dependencies != null)
            {
                foreach (TaskInList x in task.Dependencies)
                {
                    _dal.Dependency.Create(new DO.Dependency(0, task.Id, x.Id));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public void Delete(int id)
    {
        DO.Task d = _dal.Task.Read(id) ?? throw new BlDoesNotExistException($"Task with id: {id} doesn't exist");
        if (_dal.Dependency.ReadAll(x => x.DependsOnTask == id).Any()) //if there are tasks that depend on it
            throw new BlCanNotDeleteException("task can't be deleted because there are tasks dependent it");
        _dal.Task.Delete(id);
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
            RequiredEffortTime = d.RequiredEffortTime, 
            Deliverables = d.Deliverables, 
            Remarks = d.Remarks, Copmlexity = (BO.EngineerExperience)d.Complexity};
        b.Status = CalculateStatus(d);
        DO.Engineer? engineer = _dal.Engineer.Read(d.EngineerId);
        b.Engineer = (engineer == null) ? null : new BO.EngineerInTask { Id = engineer.Id, Name = engineer.Name };
        if (b.ScheduledDate != null)
        {
            if (b.StartDate != null && b.StartDate > b.ScheduledDate)
                b.ForecastDate = b.StartDate + b.RequiredEffortTime;
            else
                b.ForecastDate = b.ScheduledDate + b.RequiredEffortTime;
        }
        else
            b.ForecastDate = null;
        IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll();
        b.Dependencies = (from item in dependencies
                          where item.DependentTask == id
                          let task = _dal.Task.Read(item.DependsOnTask) ?? 
                               throw new BlDoesNotExistException($"Task with ID={item.DependsOnTask} doesn't exist")
                          select new BO.TaskInList
                          {
                              Id = task.Id,
                              Alias = task.Alias,
                              Description = task.Description,
                              Status = CalculateStatus(task)

                          }).ToList();
                         
                //TODO: Milestone
        return b;
    }

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter == null)
            return from d in _dal.Task.ReadAll()
                   select Read(d.Id);
        return from d in _dal.Task.ReadAll()
               let b = Read(d.Id)
               where filter(b)
               select b;
    }

    public void Update(BO.Task task)
    {
        try
        {
            if (task.Id <= 0)
                throw new BlInvalidInputException("Id isn't a positive number");
            if (task.Alias == "")
                throw new BlInvalidInputException("Alias can't be empty");
            if (_dal.Task.Read(task.Id) == null)
                throw new BlDoesNotExistException($"Task with id: {task.Id} doesn't exist");
            _dal.Task.Update(BoToDo(task));
            List<DO.Dependency> dependencies = _dal.Dependency.ReadAll(x => x.DependentTask == task.Id).ToList();
            foreach (TaskInList item in task.Dependencies) //add all new dependencies
            {
                if (!dependencies.Exists(x => x.DependsOnTask == item.Id))
                    _dal.Dependency.Create(new DO.Dependency(0, task.Id, item.Id));
            }
            foreach (DO.Dependency dep in dependencies) //delete all dependecies that aren't in the list anymore
            {
                if (!task.Dependencies.Exists(x => x.Id == dep.DependsOnTask))
                    _dal.Dependency.Delete(dep.Id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public void UpdateTaskDate(int id, DateTime date)
    {
        try
        {
            DO.Task task = _dal.Task.Read(id) ?? throw new BlDoesNotExistException($"Task with id: {id} doesn't exist");

            IEnumerable<BO.Task> previous = from item in _dal.Dependency.ReadAll()
                                            where item.DependentTask == id
                                            select Read(item.DependsOnTask); //get all previous tasks
            if (previous.Any(x => x.ScheduledDate == null))
                throw new BlTaskDateException("previous tasks don't have a scheduled date");
            if (previous.Any(x => x.ForecastDate > date))
                throw new BlTaskDateException("previous tasks' forcast date is later than the given date");

            _dal.Task.Update(task with { ScheduledDate = date });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// caculates the Status of the task received according to its dates
    /// </summary>
    /// <param name="b">the DO task that the status of is calculated</param>
    /// <returns>the status of the task</returns>
    public Status CalculateStatus (DO.Task b)
    {
        //InJeopardy?
        if (b.ScheduledDate == null)
            return Status.Unscheduled;
        if (b.StartDate == null)
            return Status.Scheduled;
        if (b.CompleteDate == null)
            return Status.OnTrack;
        return Status.Done;
    }

    /// <summary>
    /// turns a BO task into a DO task
    /// </summary>
    /// <param name="task">a BO task</param>
    /// <returns>a DO task that has the properties of the BO task</returns>
    public DO.Task BoToDo (BO.Task task) 
    {
        int engineerId = (task.Engineer == null) ? 0 : task.Engineer.Id;
        if (engineerId != 0 && !_dal.Engineer.ReadAll().Any(x => x.Id == engineerId)) //check if the given id exists
            throw new BlDoesNotExistException($"Engineer with ID={engineerId} doesn't exist");
        return new DO.Task(task.Id, task.Alias, task.Description, false, task.CreatedAtDate,
                task.ScheduledDate, task.StartDate, task.RequiredEffortTime, task.DeadlineDate, task.CompleteDate,
                task.Deliverables, task.Remarks, engineerId, (DO.EngineerExperience)task.Copmlexity);
    }
}
