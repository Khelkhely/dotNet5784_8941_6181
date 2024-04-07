namespace BlApi;
using DalTest;
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
    void StartNewTask(int eId, int tId);

    /// <summary>
    /// registers an engineer finishing a task and updates the data accordingly
    /// </summary>
    /// <param name="eId">the Id of the engineer that completed the task</param>
    /// <param name="tId">the Id of the task that was completed</param>
    void FinishTask(int eId, int tId);

    /// /// <summary>
    /// Checks if there are previous unfinished tasks that the task depends on
    /// </summary>
    /// <param name="eId">id of the task we want to check</param>
    /// <returns>true if there is previous task and false if there not</returns>
    bool HasPrevTask(int tId);

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
    public void InitializeDB();
    public void ResetDB();

    #region

    DateTime Clock { get; }
    void AddYear();
    void AddMonth();
    void AddDay();
    void AddHour();
    void ResetTime();
    #endregion

}
