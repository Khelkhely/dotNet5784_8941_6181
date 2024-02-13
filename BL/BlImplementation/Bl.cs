using BlApi;
using BO;

namespace BlImplementation;

//internal?
public
 class Bl : IBl
{
    private static DalApi.IDal _dal = Factory.Get;

    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public static void startNewTask()
    {
        Console.WriteLine("Enter engineer's Id: \t");
        if (int.TryParse(Console.ReadLine(), out int eId) == false)
            throw new BlTryParseFailedException("parsing failed");
        if (_dal.Engineer.Read(eId) == null)
            throw new BO.BlDoesNotExistException($"engineer with id {eId} does not exist.");

        if (Tools.EngineerIsAvailabe(eId, _dal) == false)
            throw new Exception("The engineer is not available");//!!!!!!!!!!!!!!!11

        Console.WriteLine("Enter task's Id that you want to start working on: \t");
        if (int.TryParse(Console.ReadLine(), out int tId) == false)
            throw new BlTryParseFailedException("parsing failed");
        if (_dal.Task.Read(tId) == null)
            throw new BO.BlDoesNotExistException($"task with id {tId} does not exist.");

        if (_dal.Task.Read(tId)!.EngineerId != eId)
            throw new Exception($"The task with id {tId} does not belong to the engineer with the id {eId}.");//!!!!!!!!!!!11
        if (_dal.Task.Read(tId)!.Complexity > _dal.Engineer.Read(eId)!.Level)
            throw new Exception("the engineer can't work on this task because his level is low");

        DO.Task tmpTask = _dal.Task.Read(tId)! with { StartDate = DateTime.Now };
        _dal.Task.Delete(tId);
        _dal.Task.Create(tmpTask);
    }
    public static void finishTask()
    {
        Console.WriteLine("Enter engineer's Id: \t");
        if (int.TryParse(Console.ReadLine(), out int eId) == false)
            throw new BlTryParseFailedException("parsing failed");
        if (_dal.Engineer.Read(eId) == null)
            throw new BO.BlDoesNotExistException($"engineer with id {eId} does not exist.");

        Console.WriteLine("Enter the task's Id you finished: \t");
        if (int.TryParse(Console.ReadLine(), out int tId) == false)
            throw new BlTryParseFailedException("parsing failed");
        if (_dal.Task.Read(tId) == null)
            throw new BO.BlDoesNotExistException($"task with id {tId} does not exist.");

        //_dal.Task.Read(tId).CompleteDate= DateTime.Now;
        DO.Task tmpTask = _dal.Task.Read(tId)! with { CompleteDate = DateTime.Now };
        _dal.Task.Delete(tId);
        _dal.Task.Create(tmpTask);
    }

}
