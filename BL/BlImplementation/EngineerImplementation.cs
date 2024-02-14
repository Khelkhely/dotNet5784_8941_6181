using BlApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;
    public void Create(BO.Engineer engineer)
    {
        if ((engineer.Id <= 0) ||
            (engineer.Name == null) || (engineer.Name == "") ||
            (engineer.Email == null) || !(engineer.Email.EndsWith("@gmail.com")) ||
            (engineer.Cost == null) || (engineer.Cost < 0))
            throw new BO.BlInvalidDataException($"The data of the engineer with the ID={engineer.Id} is invalid");
        try
        {
            DO.Engineer doEngineer = new DO.Engineer
                (engineer.Id,
                engineer.Name,
                engineer.Email,
                (DO.EngineerExperience)engineer.Level,
                engineer.Cost);
            _dal.Engineer.Create(doEngineer);

        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={engineer.Id} already exists", ex);
        }
    }
    public void Delete(int id)
    {
        IEnumerable<DO.Task?> doTasks = _dal.Task.ReadAll();
        var groups = from task in doTasks
                     group task by task.EngineerId into gs
                     select gs;
        var matching = groups.FirstOrDefault(x => x.Key == id);
        if (matching != null && matching.Any(item =>
               (item!.StartDate != null && item!.CompleteDate == null) ||
               (item!.CompleteDate > DateTime.Now)))
            throw new BO.BlCanNotDeleteException("The engineer is in the middle of working on a task, so he couldn't be deleted");
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exists", ex);
        }
    }

    public BO.Engineer Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null) { throw new BO.BlDoesNotExistException($"Engineer with Id = {id} does not exist."); }

        IEnumerable<DO.Task> doTasks = _dal.Task.ReadAll()!;
        DO.Task? doTask = doTasks.FirstOrDefault(item => (item.EngineerId == id
                                                          && item.StartDate != null
                                                          && item.CompleteDate == null)); //task the engineer is still working on
        if (doTask == null)
            return new BO.Engineer
            {
                Id = id,
                Name = doEngineer!.Name,
                Email = doEngineer!.Email,
                Level = (BO.EngineerExperience)doEngineer!.Level,
                Cost = doEngineer!.Cost,
                Task = null
            };
        BO.TaskInEngineer? boTask = new BO.TaskInEngineer
        {
            Id = doTask.Id,
            Alias = doTask.Alias
        };
        return new BO.Engineer
        {
            Id = id,
            Name = doEngineer!.Name,
            Email = doEngineer!.Email,
            Level = (BO.EngineerExperience)doEngineer!.Level,
            Cost = doEngineer!.Cost,
            Task = boTask
        };
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        if (filter == null)
            return from engineer in _dal.Engineer.ReadAll()
                   select Read(engineer.Id);
        return from doEngineer in _dal.Engineer.ReadAll()
               let boEngineer = Read(doEngineer.Id)
               where filter(boEngineer)
               select boEngineer;
    }

    public void Update(BO.Engineer engineer)
    {
        if ((engineer.Id < 0) ||
            (engineer.Name == null) || (engineer.Name == "") ||
            (engineer.Email == null) || !(engineer.Email.EndsWith("@gmail.com")) ||
            (engineer.Cost == null) || (engineer.Cost < 0))
            throw new BO.BlInvalidDataException($"The data of the engineer with the ID={engineer.Id} is invalid");

        if (engineer.Task != null)
        {
            DO.Task? doTask = _dal.Task.ReadAll().FirstOrDefault<DO.Task?>(item => item!.Id == engineer.Task!.Id)! with { EngineerId = engineer.Id };
            try
            {
                _dal.Task.Update(doTask);

            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={engineer.Task!.Id} does not exists", ex);
            }
        }


        DO.Engineer doEngineer = new DO.Engineer();
        if ((_dal.Engineer.Read(engineer.Id) != null) &&
            (DO.EngineerExperience)engineer.Level < _dal.Engineer.Read(engineer.Id)!.Level)
        {
            doEngineer = new DO.Engineer
            {
                Name = engineer.Name,
                Email = engineer.Email,
                Cost = engineer.Cost
            };
        }
        else
        {
            doEngineer = new DO.Engineer
            {
                Name = engineer.Name,
                Email = engineer.Email,
                Cost = engineer.Cost,
                Level = (DO.EngineerExperience)engineer.Level
            };
        }
        BO.Engineer en = new BO.Engineer();
        try
        {
            _dal.Engineer.Update(doEngineer);

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={engineer.Id} does not exists", ex);
        }
    }

    public bool EngineerIsAvailabe(int id)
    {
        if (_dal.Engineer.Read(id) == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exists");
        var tasks = _dal.Task.ReadAll(x => x.EngineerId == id);
        //checks if there is a task that the engineer is assigned to, had started, but hadn't finished
        if (tasks.Any(x => x.StartDate != null && x.CompleteDate == null))
            return false;
        return true;
    }
}
