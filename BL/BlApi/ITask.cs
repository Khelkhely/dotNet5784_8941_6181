namespace BlApi;
/// <summary>
/// 
/// </summary>
public interface ITask
{
    /// <summary>
    /// returns a collection of all BO.Tasks that exist and satisfy a condition if given
    /// </summary>
    /// <param name="filter">a boolean function that determines for each task if it satisfies the condition or not</param>
    /// <returns>an IEnumerable of BO tasks</returns>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);

    /// <summary>
    /// returns a BO.Task according to the Id
    /// </summary>
    /// <param name="id">the Id of the task that will be returned</param>
    /// <returns>a complete BO.Task</returns>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no task with this Id</exception>
    public BO.Task Read(int id);

    /// <summary>
    /// adds a new task to the data layer
    /// </summary>
    /// <param name="task">the new task that will be added</param>
    /// <exception cref="BO.BlInvalidDataException">thrown if the data of the task is invalid</exception>
    /// <exception cref="BO.BlDoesNotExistException">thrown if the data contains Ids of objects that don't exist</exception>
    public void Create(BO.Task task);

    /// <summary>
    /// deletes the task with the Id given
    /// </summary>
    /// <param name="id">the Id of the task</param>
    /// <exception cref="BO.BlCanNotDeleteException">thrown if the task has other tasks that depend on it and thus can't be deleted</exception>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no task with such an Id</exception>
    public void Delete(int id);

    /// <summary>
    /// Updates the data of a task in the data layer to the data of the task received
    /// </summary>
    /// <param name="task">the task to be updated with the new data</param>
    /// <exception cref="BO.BlInvalidDataException">thrown if the new data is invalid</exception>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no task with the Id of the given task</exception>
    public void Update(BO.Task task);

    /// <summary>
    /// set the scheduled starting date of a task
    /// </summary>
    /// <param name="id">the Id of the task to update</param>
    /// <param name="date">the scheduled date of the task</param>
    /// <param name="projectStartDate">the scheduled starting date of the project</param>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no task with the given Id</exception>
    /// <exception cref="BO.BlTaskDateException">thrown if the task scheduled date can't be the updated to the given date</exception>
    public void UpdateTaskDate(int id, DateTime date, DateTime projectStartDate);
}
