using System;
using System.Reflection;

namespace BO;

static class Tools
{
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        foreach (PropertyInfo item in t.GetType().GetProperties())
        {
            str += "\n" + item.Name + ": " + item.GetValue(t, null);
        }
        return str;
    }

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
}
