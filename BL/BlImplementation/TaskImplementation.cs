using BlApi;
using System.Collections.Specialized;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;
    public void Create(BO.Task task)
    {
        //if (task.Id <= 0) //task id not received - it's automatic from the Dal layer
        //    throw new BO.BlInvalidDataException("Id isn't a positive number");
        if (task.Alias == "")
            throw new BO.BlInvalidDataException("Alias can't be empty");
        if (task.Dependencies != null) // doesn't matter because the 
            foreach (var t in task.Dependencies) //complete the data for the dependant tasks
            {
                var dTask = _dal.Task.Read(t.Id) ??
                    throw new BO.BlDoesNotExistException($"Task with id: {t.Id} doesn't exist");
                t.Alias = dTask.Alias;
                t.Description = dTask.Description;
                t.Status = BO.Tools.CalculateStatus(dTask);
            }
        //check if the engineer exists
        if (task.Engineer != null && _dal.Engineer.ReadAll().All(x => x.Id != task.Engineer.Id))
            throw new BO.BlDoesNotExistException($"Engineer with id: {task.Engineer.Id} doesn't exist");
        int newId  = _dal.Task.Create(BoToDo(task)); //doesn't throw exceptions
        if (task.Dependencies != null)
        {
            foreach (var item in task.Dependencies) //check that all the the dependent on tasks exist
            {
                if (_dal.Task.Read(item.Id) == null)
                    throw new BO.BlDoesNotExistException($"Task with id: {item.Id} doesn't exist");
            }
            //create the dependencies
            task.Dependencies.ForEach(x => _dal.Dependency.Create(new DO.Dependency(0, newId, x.Id)));
        }
    }

    public void Delete(int id)
    {
        if (_dal.Dependency.ReadAll(x => x.DependsOnTask == id).Any()) //if there are tasks that depend on it
            throw new BO.BlCanNotDeleteException("task can't be deleted because there are tasks dependent it");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id: {id} doesn't exist", ex);
        }
    }

    public BO.Task Read(int id)
    {
        DO.Task d = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with id: {id} doesn't exist");
        BO.Task b = new BO.Task()
        {
            Id = id,
            Alias = d.Alias,
            Description = d.Description,
            CreatedAtDate = d.CreatedAtDate,
            ScheduledDate = d.ScheduledDate,
            StartDate = d.StartDate,
            CompleteDate = d.CompleteDate,
            RequiredEffortTime = d.RequiredEffortTime,
            Deliverables = d.Deliverables,
            Remarks = d.Remarks,
            Copmlexity = (BO.EngineerExperience)d.Complexity
        };
        b.Status = BO.Tools.CalculateStatus(d);
        b.ForecastDate = BO.Tools.CalculateForcast(d);
        DO.Engineer? engineer = _dal.Engineer.Read(d.EngineerId);
        b.Engineer = (engineer == null) ? null : new BO.EngineerInTask { Id = engineer.Id, Name = engineer.Name };

        IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll();
        b.Dependencies = (from item in dependencies
                          where item.DependentTask == id
                          let task = _dal.Task.Read(item.DependsOnTask) ??
                               throw new BO.BlDoesNotExistException($"Task with ID={item.DependsOnTask} doesn't exist")
                          select new BO.TaskInList
                          {
                              Id = task.Id,
                              Alias = task.Alias,
                              Description = task.Description,
                              Status = BO.Tools.CalculateStatus(task)

                          }).OrderBy(x => x.Id).ToList();
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
        if (task.Id <= 0)
            throw new BO.BlInvalidDataException("Id isn't a positive number");
        if (task.Alias == "")
            throw new BO.BlInvalidDataException("Alias can't be empty");
        try
        {
            _dal.Task.Update(BoToDo(task));
            List<DO.Dependency> dependencies = _dal.Dependency.ReadAll(x => x.DependentTask == task.Id).ToList();
            if (task.Dependencies != null)
            {
                //get a list of dependencies that need to be added
                var toAdd = from t in task.Dependencies
                            where !dependencies.Exists(x => x.DependsOnTask == t.Id)
                            select new DO.Dependency(0, task.Id, t.Id);
                foreach (DO.Dependency x in toAdd) //add the new dependencies to the data layer
                {
                    _dal.Dependency.Create(x);
                }

                //get a list of the id of all the dependencies that need to be deleted
                var toDelete = from dep in dependencies
                               where !task.Dependencies.Exists(t => t.Id == dep.DependsOnTask)
                               select dep.Id;
                foreach (int id in toDelete) //delete the dependencies chosen earlier
                {
                    _dal.Dependency.Delete(id);
                }
            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id: {task.Id} doesn't exist", ex);
        }
    }

    public void UpdateTaskDate(int id, DateTime date)
    {
        DO.Task task = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with id: {id} doesn't exist");
        IEnumerable<BO.Task> previous = from item in _dal.Dependency.ReadAll()
                                        where item.DependentTask == id
                                        select Read(item.DependsOnTask); //get all previous tasks
        if (previous.Any(x => x.ScheduledDate == null))
            throw new BO.BlTaskDateException("previous tasks don't have a scheduled date");
        if (previous.Any(x => x.ForecastDate > date))
            throw new BO.BlTaskDateException("previous tasks' forcast date is later than the given date");
        try
        {
            _dal.Task.Update(task with { ScheduledDate = date });
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id: {id} doesn't exist", ex);
        }
    }



    /// <summary>
    /// turns a BO task into a DO task
    /// </summary>
    /// <param name="task">a BO task</param>
    /// <returns>a DO task that has the properties of the BO task</returns>
    public DO.Task BoToDo(BO.Task task)
    {
        int engineerId = (task.Engineer == null) ? 0 : task.Engineer.Id;
        if (engineerId != 0 && !_dal.Engineer.ReadAll().Any(x => x.Id == engineerId)) //check if the given id exists
            throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} doesn't exist");
        return new DO.Task(task.Id, task.Alias, task.Description, task.CreatedAtDate,
                task.ScheduledDate, task.StartDate, task.RequiredEffortTime, task.CompleteDate,
                task.Deliverables, task.Remarks, engineerId, (DO.EngineerExperience)task.Copmlexity);
    }
}
