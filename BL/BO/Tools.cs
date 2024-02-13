using DO;
using System;
using System.Reflection;

namespace BO;

static class Tools
{
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        if (t != null)
        {
            foreach (PropertyInfo item in t.GetType().GetProperties())
            {
                str += "\n" + item.Name + ": " + item.GetValue(t, null);
            }
        }
        return str;
    }

    //public static string ToStringList<T>(this T t) where T : System.Collections.IEnumerable
    //{
    //    string str = "";
    //    foreach (var item in t) 
    //    {
    //        str += item?.ToString();
    //        str += ", ";
    //    }
    //    str.Trim(' ', ',');
    //    return str;
    //}

    /// <summary>
    /// caculates the Status of the task received according to its dates
    /// </summary>
    /// <param name="task">the DO task that the status of is calculated</param>
    /// <returns>the status of the task</returns>
    public static Status CalculateStatus(DO.Task task)
    {
        if (task.CompleteDate != null)
            return Status.Done;
        if (task.StartDate != null)
            return Status.OnTrack;
        if (task.ScheduledDate != null)
            return Status.Scheduled;
        return Status.Unscheduled;
        //InJeopardy?
    }

    public static DateTime? CalculateForcast(DO.Task task)
    {
        if (task.ScheduledDate != null)
        {
            if (task.StartDate != null && task.StartDate > task.ScheduledDate)
                return task.StartDate + task.RequiredEffortTime;
            else
                return task.ScheduledDate + task.RequiredEffortTime;
        }
        return null;
    }

    public static bool EngineerIsAvailabe(int id, DalApi.IDal _dal)
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
