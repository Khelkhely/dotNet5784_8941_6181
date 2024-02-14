using BlApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    /// adds a new engineer to the data layer
    /// </summary>
    /// <param name="engineer">the new engineer that will be added</param>
    /// <exception cref="BO.BlInvalidDataException">thrown if the data of the engineer is invalid</exception>
    /// <exception cref="BO.BlAlreadyExistsException">thrown if the data contains Ids of objects that already exist</exception>
    public void Create(BO.Engineer engineer)
    {
        //if the data of the engineer is invalid:
        if ((engineer.Id <= 0) ||
            (engineer.Name == null) || (engineer.Name == "") ||
            (engineer.Email == null) || !(engineer.Email.EndsWith("@gmail.com")) ||
            (engineer.Cost == null) || (engineer.Cost < 0))
            throw new BO.BlInvalidDataException($"The data of the engineer with the ID={engineer.Id} is invalid");
        try
        {
            //create new engineer with the desired values in the data layer:
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

    /// <summary>
    /// deletes an engineer from the data layer
    /// </summary>
    /// <param name="id">the id of the engineer that you want to delete</param>
    /// <exception cref="BO.BlCanNotDeleteException">thrown if an engineer with the id that received can't be deleted</exception>
    /// <exception cref="BO.BlDoesNotExistException">thrown if an engineer with the id that received does not exist</exception>
    public void Delete(int id)
    {
        IEnumerable<DO.Task?> doTasks = _dal.Task.ReadAll();//get all the tasks from the data layer
        //puts all the tasks that were assigne to the engineer (with the received id) in a group:
        var groups = from task in doTasks
                     group task by task.EngineerId into gs
                     select gs;
        var matching = groups.FirstOrDefault(x => x.Key == id);
        if (matching != null && matching.Any(item =>
               (item!.StartDate != null && item!.CompleteDate == null) ||
               (item!.CompleteDate > DateTime.Now)))//if the engineer is in the middle of working on that task
            throw new BO.BlCanNotDeleteException("The engineer is in the middle of working on a task, so he couldn't be deleted");
        try
        {
            _dal.Engineer.Delete(id);//delete the engineer from the data layer
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exists", ex);
        }
    }

    /// <summary>
    /// returns a BO.Engineer according to the Id
    /// </summary>
    /// <param name="id">the Id of the engineer that will be returned</param>
    /// <returns>a complete BO.Engineer</returns>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no engineer with the id that received</exception>
    public BO.Engineer Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id); //read from the data layer an enguneer with the id that recived

        if (doEngineer == null) { throw new BO.BlDoesNotExistException($"Engineer with Id = {id} does not exist."); }

        //finds the task (if exists) that the engineer is working on
        IEnumerable<DO.Task> doTasks = _dal.Task.ReadAll()!;
        DO.Task? doTask = doTasks.FirstOrDefault(item => (item.EngineerId == id
                                                          && item.StartDate != null
                                                          && item.CompleteDate == null)); //task the engineer is still working on
        if (doTask == null)//if there is no such task
            return new BO.Engineer
            {
                Id = id,
                Name = doEngineer!.Name,
                Email = doEngineer!.Email,
                Level = (BO.EngineerExperience)doEngineer!.Level,
                Cost = doEngineer!.Cost,
                Task = null
            };
        //else, create a TaskInEngineer with the data of the task that the engineer works on:
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

    /// <summary>
    /// returns a collection of all BO.Engineers that exist and satisfy a condition if given
    /// </summary>
    /// <param name="filter">a boolean function that determines for each engineer if it satisfies the condition or not</param>
    /// <returns>an IEnumerable of BO engineers</returns>
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

    /// <summary>
    /// Updates the data of an engineer in the data layer to the data of the engineer received
    /// </summary>
    /// <param name="engineer">the engineer to be updated with the new data</param>
    /// <exception cref="BO.BlInvalidDataException">thrown if the new data is invalid</exception>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no engineer with the Id of the given engineer</exception>
    public void Update(BO.Engineer engineer)
    {
        if ((engineer.Id < 0) ||
            (engineer.Name == null) || (engineer.Name == "") ||
            (engineer.Email == null) || !(engineer.Email.EndsWith("@gmail.com")) ||
            (engineer.Cost == null) || (engineer.Cost < 0))
            throw new BO.BlInvalidDataException($"The data of the engineer with the ID={engineer.Id} is invalid");

        if (engineer.Task != null)//if the engineer that recieved is working on a task
        {
            DO.Task? doTask = _dal.Task.ReadAll().FirstOrDefault<DO.Task?>(item => item!.Id == engineer.Task!.Id)! with { EngineerId = engineer.Id };//finds the task and updates it with the engineer's id
            try
            {
                _dal.Task.Update(doTask);

            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={engineer.Task!.Id} does not exists", ex);
            }
        }


        DO.Engineer doEngineer;
        if (_dal.Engineer.Read(engineer.Id) != null)//if an engineer with tha recived id exists in the data layer
        {
            if ((DO.EngineerExperience)engineer.Level < _dal.Engineer.Read(engineer.Id)!.Level)//if the level of the recived engineer is smaller then the previous one 
            {
                //we will keep the previous level
                doEngineer = new DO.Engineer
                {
                    Id = engineer.Id,
                    Name = engineer.Name,
                    Email = engineer.Email,
                    Cost = engineer.Cost,
                    Level = _dal.Engineer.Read(engineer.Id)!.Level
                };
            }
            else//else, we will update the level too
            {
                doEngineer = new DO.Engineer
                {
                    Id = engineer.Id,
                    Name = engineer.Name,
                    Email = engineer.Email,
                    Cost = engineer.Cost,
                    Level = (DO.EngineerExperience)engineer.Level
                };
            }
        }
        else
            throw new BO.BlDoesNotExistException($"Engineer with ID={engineer.Id} does not exists");

        try
        {
            _dal.Engineer.Update(doEngineer);

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={engineer.Id} does not exists", ex);
        }
    }

    /// <summary>
    /// help function that checks whether an engineer is available (not in the middle of doing a task)
    /// </summary>
    /// <param name="id">id of the checked engineer</param>
    /// <returns>true if the engineer is available and false if he does not</returns>
    /// <exception cref="BO.BlDoesNotExistException">thrown if there is no engineer with the Id that recived</exception>
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

