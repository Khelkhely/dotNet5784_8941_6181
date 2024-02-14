namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }

    /// <summary>
    /// returns a collection of all the tasks assigned to the engineer
    /// </summary>
    /// <param name="eId">the Id of the engineer</param>
    /// <returns>an IEnumerable collection of tasks assigned to the engineer</returns>
    public IEnumerable<BO.Task> ShowTasks(int eId);

    /// <summary>
    /// registers an engineer starting to work on a task and updates the data accordingly
    /// </summary>
    /// <param name="eId">the Id of the engineer that wants to start working on the task</param>
    /// <param name="tId">the Id of the task he wants to start</param>
    void startNewTask(int eId, int tId);

    /// <summary>
    /// registers an engineer finishing a task and updates the data accordingly
    /// </summary>
    /// <param name="eId">the Id of the engineer that completed the task</param>
    /// <param name="tId">the Id of the task that was completed</param>
    void finishTask(int eId, int tId);

    /// <summary>
    /// checks that a schedule can be made and sets the project starts date if it can
    /// </summary>
    /// <param name="date">the desired scheduled date of the project</param>
    void CreateSchedule(DateTime date);

    /// <summary>
    /// returns true if the project has a start date
    /// </summary>
    /// <returns></returns>
    bool IsScheduled();

    /// <summary>
    /// assigns the task to the engineer and updates the data accordingly
    /// </summary>
    /// <param name="engineerId"></param>
    /// <param name="taskId"></param>
    void AssignEngineer(int engineerId, int taskId);
}
