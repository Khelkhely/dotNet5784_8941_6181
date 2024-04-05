using BlApi;
using BO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace BlImplementation;
internal class Bl : IBl
{
    private static DalApi.IDal _dal = Factory.Get;

    public ITask Task => new TaskImplementation(this);

    public IEngineer Engineer => new EngineerImplementation(this);

    private static DateTime s_clock = DateTime.Now.Date;
    public DateTime Clock { get => s_clock; private set => s_clock = value; }

    /// <summary>
    /// registers an engineer starting to work on a task and updates the data accordingly
    /// </summary>
    /// <param name="eId">the Id of the engineer that wants to start working on the task</param>
    /// <param name="tId">the Id of the task he wants to start</param>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there are no enginners / tasks with the received Ids</exception>
    /// <exception cref="BlAssignmentFailedException">thrown if the engineer can't start working on the task</exception>
    public void startNewTask(int eId, int tId)
    {
       
        if (_dal.Engineer.Read(eId) == null)
            throw new BO.BlDoesNotExistException($"engineer with id {eId} does not exist.");
        if (Engineer.EngineerIsAvailabe(eId) == false)
            throw new BlAssignmentFailedException("The engineer is not available");
        if (_dal.Task.Read(tId) == null)
            throw new BO.BlDoesNotExistException($"task with id {tId} does not exist.");
        if (_dal.Task.Read(tId)!.EngineerId != eId)
            throw new BlAssignmentFailedException($"The task with id {tId} does not belong to the engineer with the id {eId}.");
        if (_dal.Task.Read(tId)!.Complexity > _dal.Engineer.Read(eId)!.Level)
            throw new BlAssignmentFailedException("the engineer can't work on this task because his level is low");
        if(HasPrevTask(eId))
            throw new BlAssignmentFailedException("the engineer has previous unfinished tasks.");
        
        DO.Task tmpTask = _dal.Task.Read(tId)! with { StartDate = Clock };
        _dal.Task.Update(tmpTask);

    }
    
    /// <summary>
    /// Checks if there are previous unfinished tasks
    /// </summary>
    /// <param name="eId">id of the task we want to check</param>
    /// <returns>true if there is previous task and false if there not</returns>
    public bool HasPrevTask(int eId)
    {
        var dependincies = _dal.Dependency.ReadAll(dependency => dependency.DependentTask == eId); 
        foreach (var _task in dependincies)
        {
            if (_dal.Task.Read(_task.DependsOnTask)!.CompleteDate is null)
                return true;
        }
        return false;
    }

    /// <summary>
    ///  registers an engineer finishing a task and updates the data accordingly
    /// </summary>
    /// <param name="eId">the Id of the engineer that completed the task</param>
    /// <param name="tId">the Id of the task that was completed</param>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there are no enginners / tasks with the received Ids</exception>
    public void finishTask(int eId, int tId)
    {
       
        if (_dal.Engineer.Read(eId) == null)
            throw new BO.BlDoesNotExistException($"engineer with id {eId} does not exist.");

        if (_dal.Task.Read(tId) == null)
            throw new BO.BlDoesNotExistException($"task with id {tId} does not exist.");

        DO.Task tmpTask = _dal.Task.Read(tId)! with { EngineerId = 0, CompleteDate = Clock };
        _dal.Task.Update(tmpTask);
    }

    /// <summary>
    /// returns a collection of all the tasks assigned to the engineer
    /// </summary>
    /// <param name="eId">the Id of the engineer</param>
    /// <returns>an IEnumerable collection of tasks assigned to the engineer</returns>
    public IEnumerable<BO.Task> ShowTasks(int eId)
    {
        if (_dal.Engineer.Read(eId) == null)
            throw new BlDoesNotExistException($"engineer with id {eId} does not exist.");
        return Task.ReadAll(x => x.Engineer != null && x.Engineer.Id == eId);
    }

    /// <summary>
    /// checks that a schedule can be made and sets the project starts date if it can
    /// </summary>
    /// <param name="date">the desired scheduled date of the project</param>
    /// <exception cref="BlSchedulingFailedException">thrown if the tasks' scheduled dates don't allow scheduling the given project start date</exception>
    public void CreateSchedule(DateTime date) 
    {
        var tasks = Task.ReadAll();
        //check that all tasks are scheduled
        if (tasks.Any(x => x.Status == BO.Status.Unscheduled)) 
            throw new BlSchedulingFailedException("Not all tasks are scheduled");
        //check that none of the scheduled dates are before the start of the project
        if (tasks.Any(x => x.ScheduledDate < date)) 
            throw new BlSchedulingFailedException("Project starting date is before the task's scheduled date");
        //save the start date of the project in the configuration data
        _dal.StartDate = date;
    }

    /// <summary>
    /// returns true if the project has a start date
    /// </summary>
    /// <returns></returns>
    public bool IsScheduled() => _dal.StartDate != null;

    /// <summary>
    /// assigns the task to the engineer and updates the data accordingly
    /// </summary>
    /// <param name="engineerId"></param>
    /// <param name="taskId"></param>
    public void AssignEngineer(int engineerId, int taskId)
    {
        var engineer = Engineer.Read(engineerId);
        var task = Task.Read(taskId);
        task.Engineer = new BO.EngineerInTask() { Id = engineer.Id, Name = engineer.Name };
        Task.Update(task); //update the task
        //the engineer doesn't need to be updated because it doesn't have a task property in dal
    }

    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.Reset();

    public void AddYear()
    {
        s_clock = s_clock.AddYears(1);
    }

    public void AddMonth()
    {
        s_clock = s_clock.AddMonths(1);
    }

    public void AddDay()
    {
        s_clock = s_clock.AddDays(1);
    }

    public void AddHour()
    {
        s_clock = s_clock.AddHours(1);
    }

    public void ResetTime()
    {
        s_clock = DateTime.Now.Date;
    }
}
