using BlApi;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    /// adds a new task to the data layer
    /// </summary>
    /// <param name="task">the new task that will be added</param>
    /// <exception cref="BO.BlInvalidDataException">thrown if the data of the task is invalid</exception>
    /// <exception cref="BO.BlDoesNotExistException">thrown if the data contains Ids of objects that don't exist</exception>
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
                t.Status = CalculateStatus(dTask);
            }
        //check if the engineer exists
        if (task.Engineer != null && _dal.Engineer.Read(task.Engineer.Id) == null)
            throw new BO.BlDoesNotExistException($"Engineer with id: {task.Engineer.Id} doesn't exist");
        int newId  = _dal.Task.Create(BoToDo(task) with { CreatedAtDate = DateTime.Now }); //doesn't throw exceptions
        if (task.Dependencies != null)
        {
            foreach (var item in task.Dependencies) //check that all the the dependent-on tasks exist
            {
                if (_dal.Task.Read(item.Id) == null)
                    throw new BO.BlDoesNotExistException($"Task with id: {item.Id} doesn't exist");
            }
            //create the dependencies
            task.Dependencies.ForEach(x => _dal.Dependency.Create(new DO.Dependency(0, newId, x.Id)));
        }
    }

    /// <summary>
    /// deletes the task with the Id given
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlCanNotDeleteException">thrown if the task has other tasks that depend on it and thus can't be deleted</exception>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no task with such an Id</exception>
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

    /// <summary>
    /// returns a BO.Task according to the Id
    /// </summary>
    /// <param name="id">the Id of the task that will be returned</param>
    /// <returns>a complete BO.Task</returns>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no task with this Id</exception>
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
        b.Status = CalculateStatus(d);
        b.ForecastDate = CalculateForcast(d);
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
                              Status = CalculateStatus(task)

                          }).OrderBy(x => x.Id).ToList();
        return b;
    }

    /// <summary>
    /// returns a collection of all BO.Tasks that exist and satisfy a condition if given
    /// </summary>
    /// <param name="filter">a boolean function that determines for each task if it satisfies the condition or not</param>
    /// <returns>an IEnumerable of BO tasks</returns>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter == null)
            return from d in _dal.Task.ReadAll()
                   orderby d.Id
                   select Read(d.Id);
        return from d in _dal.Task.ReadAll()
               let b = Read(d.Id)
               where filter(b)
               orderby b.Id
               select b;
    }

    /// <summary>
    /// Updates the data of a task in the data layer to the data of the task received
    /// </summary>
    /// <param name="task">the task to be updated with the new data</param>
    /// <exception cref="BO.BlInvalidDataException">thrown if the new data is invalid</exception>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no task with the Id of the given task</exception>
    public void Update(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BO.BlInvalidDataException($"Id {task.Id} isn't a positive number");
        if (task.Alias == "")
            throw new BO.BlInvalidDataException("Alias can't be empty");
        if (task.Engineer != null)
        {
            //check that the engineer exists
            DO.Engineer engineer = _dal.Engineer.Read(task.Engineer.Id) ??
                throw new BO.BlDoesNotExistException($"Engineer with id: {task.Engineer.Id} doesn't exist");

            //check that the engineer is advanced enough to be assigned the task
            if ((int)engineer.Level < (int)task.Copmlexity)
                throw new BO.BlInvalidDataException($"Engineer {engineer.Name} isn't experienced enough for the task");
        }
        try
        {
            //update all changes in the dependencies of the task
            if (task.Dependencies != null)
            {
                List<DO.Dependency> dependencies = _dal.Dependency.ReadAll(x => x.DependentTask == task.Id).ToList();
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
            else 
            {
                foreach (var dep in _dal.Dependency.ReadAll(x => x.DependentTask == task.Id))
                {
                    _dal.Dependency.Delete(dep.Id);
                }
            }//if there are no dependencies, erase all of the tasks dependencies that were there before

            //Update the task itself
            _dal.Task.Update(BoToDo(task));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id: {task.Id} doesn't exist", ex);
        }
    }

    /// <summary>
    /// set the scheduled starting date of a task
    /// </summary>
    /// <param name="id">the Id of the task to update</param>
    /// <param name="date">the scheduled date of the task</param>
    /// <param name="projectStartDate">the scheduled starting date of the project</param>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no task with the given Id</exception>
    /// <exception cref="BO.BlTaskDateException">thrown if the task scheduled date can't be the updated to the given date</exception>
    public void UpdateTaskDate(int id, DateTime date, DateTime projectStartDate)
    {
        DO.Task task = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with id: {id} doesn't exist");
        //get all previous tasks (DO)
        IEnumerable<DO.Task> previous = from item in _dal.Dependency.ReadAll()
                                        where item.DependentTask == id
                                        select _dal.Task.Read(item.DependsOnTask) ??
                                            throw new BO.BlDoesNotExistException
                                            ($"Task with id: {item.DependsOnTask} doesn't exist"); 
        if (previous.Count() == 0) //if there are no previous tasks
        {
            if (date < projectStartDate)
                throw new BO.BlTaskDateException("scheduled date is earlier than the project's scheduled date");
        }
        else //if there are previous tasks
        {
            if (previous.Any(x => x.ScheduledDate == null))
                throw new BO.BlTaskDateException("previous tasks don't have a scheduled date");
            if (previous.Any(x => CalculateForcast(x) > date))
                throw new BO.BlTaskDateException("previous tasks' forcast date is later than the given date");
        }
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
    /// converts a BO task into a DO task
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


    /// <summary>
    /// caculates the Status of the task received according to its dates
    /// </summary>
    /// <param name="task">the DO task that the status of is calculated</param>
    /// <returns>the status of the task</returns>
    public static BO.Status CalculateStatus(DO.Task task)
    {
        if (task.CompleteDate != null)
            return BO.Status.Done;
        if (task.StartDate != null)
            return BO.Status.OnTrack;
        if (task.ScheduledDate != null)
            return BO.Status.Scheduled;
        return BO.Status.Unscheduled;
    }
    /// <summary>
    /// caculates the forcast date of the task received according to its dates. if the task is unscheduled, returns null
    /// </summary>
    /// <param name="task">the DO task that the forcast date of is calculated</param>
    /// <returns>the scheduled / start date of the task + the required effort time</returns>
    public static DateTime? CalculateForcast(DO.Task task)
    {
        if (task.ScheduledDate != null)
        {
            if (task.StartDate != null && task.StartDate > task.ScheduledDate)
                return task.StartDate + task.RequiredEffortTime;
            else
                return task.ScheduledDate + task.RequiredEffortTime;
        }
        return null;
    }

}
