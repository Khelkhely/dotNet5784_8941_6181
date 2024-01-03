namespace DO;
/// <summary>
/// The dependency between the tasks, i.e. which task can only be performed after the completion of a previous task
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number)</param>
/// <param name="DependentTask">Number of the task that depends on this task</param>
/// <param name="DependsOnTask">Number of the task it depends on</param>
public record Dependency
(
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
)
{
    public Dependency() : this(0) { } //empty ctor
}
