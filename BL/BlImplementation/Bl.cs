using BlApi;

namespace BlImplementation;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();


    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    /// checks that a schedule can be made and sets the project starts date if it can
    /// </summary>
    /// <param name="date"></param>
    /// <exception cref="Exception"></exception>
    public void CreateSchedule(DateTime date) 
    {
        var tasks = Task.ReadAll();
        //check that all tasks are scheduled
        if (tasks.Any(x => x.Status == BO.Status.Unscheduled)) 
            throw new Exception();
        //check that none of the scheduled dates are before the start of the project
        if (tasks.Any(x => x.ScheduledDate < date)) 
            throw new Exception();
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
}
