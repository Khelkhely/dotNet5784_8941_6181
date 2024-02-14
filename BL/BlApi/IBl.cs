namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }

    void startNewTask(int eId, int tId);
    void finishTask(int eId, int tId);
    void CreateSchedule(DateTime date);
    bool IsScheduled();
    void AssignEngineer(int engineerId, int taskId);
}
