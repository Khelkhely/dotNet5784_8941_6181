namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }

    void startNewTask();
    void finishTask();
    void CreateSchedule(DateTime date);
    bool IsScheduled();
    void AssignEngineer(int engineerId, int taskId);
}
