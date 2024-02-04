using BlApi;
using DalApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlImplementation;

internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = Factory.Get;
    public void Create(BO.Engineer engineer)
    {
        if ((engineer.Id < 0) ||
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
        
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={engineer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        IEnumerable<DO.Task?> doTasks = _dal.Task.ReadAll();
        foreach (var item in doTasks)
            if (item!.EngineerId == id &&
               (item!.StartDate != null && item!.CompleteDate == null) ||
               (item!.CompleteDate > DateTime.Now))
                throw new BO.BlEngineerDeletionFailedException("The engineer is in the middle of working on a task, so he couldn't be deleted");
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

        IEnumerable<DO.Task?> doTasks = _dal.Task.ReadAll();
        DO.Task? doTask = doTasks.FirstOrDefault<DO.Task?>(item => (item!.EngineerId == id
                                                                   && item!.StartDate != null
                                                                   && item!.CompleteDate == null));
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
            Id = doTask.EngineerId,
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
        
    

    public IEnumerable<BO.Engineer?> ReadAll()
    {
        IEnumerable <DO.Engineer?> doEngineers = _dal.Engineer.ReadAll(item => item.Level == DO.EngineerExperience.Beginner);
        if (doEngineers == null)
            throw new BO.BlDoesNotExistException("No engineer exist in the list.");

        IEnumerable<DO.Task?> doTasks = _dal.Task.ReadAll();

        IEnumerable<BO.Engineer?> boEngineers = from DO.Engineer doEngineer in doEngineers
                                                where doTasks.FirstOrDefault<DO.Task?>
                                                                   (item => (item!.EngineerId == doEngineer.Id
                                                                   && item!.StartDate != null
                                                                   && item!.CompleteDate == null))
                                                                   != null
                                                select new BO.Engineer 
                                                {
                                                    Id = doEngineer!.Id,
                                                    Name = doEngineer!.Name,
                                                    Email = doEngineer!.Email,
                                                    Level = (BO.EngineerExperience)doEngineer!.Level,
                                                    Cost = doEngineer!.Cost,
                                                    Task = new BO.TaskInEngineer
                                                    {
                                                        Id = _dal.Task.Read(doEngineer!.Id)!.Id,
                                                        Alias = _dal.Task.Read(doEngineer!.Id)!.Alias
                                                    }

                                                };
        //מה קורה אם אין טאסק כזה?
                                                        
        //foreach (var item in doEngineers)  doEngineers.Select<DO.Engineer?>(item => item.Id == 123456)
       // Where<DO.Engineer?>(item => item.Id == 123456)


        return boEngineers;
    }

    public void Update(BO.Engineer engineer)
    {
        if ((engineer.Id < 0) ||
            (engineer.Name == null) || (engineer.Name == "") ||
            (engineer.Email == null) || !(engineer.Email.EndsWith("@gmail.com")) ||
            (engineer.Cost == null) || (engineer.Cost < 0)) 
            throw new BO.BlInvalidDataException($"The data of the engineer with the ID={engineer.Id} is invalid");

        DO.Task? doTask = _dal.Task.ReadAll().FirstOrDefault<DO.Task?>(item => item!.Id == engineer.Task!.Id)! with { EngineerId = engineer.Id };
        _dal.Task.Update(doTask);

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
}
