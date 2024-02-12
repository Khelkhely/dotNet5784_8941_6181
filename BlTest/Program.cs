using BO;
using System;
using System.Security.Cryptography.X509Certificates;

namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        Console.WriteLine("Would you like to create Initial data? Y/N\t");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();
        int choice;
        do
        {
            Console.WriteLine("Choose an entity you want to check." +
                "0: Exit main menu" +
                "1: Tasks" +
                "2: Engineer");
            choice = int.TryParse(Console.ReadLine(), out int value) ? value :
                throw new BO.BlTryParseFailedException("Parsing failed");
            switch (choice)
            {
                case 1:
                    TaskMenu();
                    break;
                case 2:
                    break;
                case 0:
                    break;
                default:
                    throw new BO.BlOptionDoesntExistException("There is no such option in the menu");
            }
        }
        while (choice != 0);
    }

    private static void TaskMenu()
    {
        Console.WriteLine("Choose an action:" +
            "0: return to main menu" +
            "1: add new task" +
            "2: show all tasks" +
            "3: show task" +
            "4: update a task" +
            "5: assign a task to an engineer" +
            "6: delete a task" +
            "7: create a schedule for the project");
        int choice = int.TryParse(Console.ReadLine(), out int value) ? value :
                throw new BO.BlTryParseFailedException("Parsing failed");
        int tmp;
        switch (choice)
        {
            case 1:
                AddTask();
                break;
            case 2:
                foreach (var item in s_bl.Task.ReadAll())
                {
                    Console.WriteLine(item);
                }
                break;
            case 3:
                Console.WriteLine("Enter task Id:");
                tmp = int.TryParse(Console.ReadLine(), out int num) ? num :
                    throw new BO.BlTryParseFailedException("Parsing failed");
                Console.WriteLine(s_bl.Task.Read(tmp));
                break;
            case 4:
                UpdateTask();
                break;
            case 5:
                AssignEngineer();
                break;
            case 6:
                Console.WriteLine("Enter task Id:");
                tmp = int.TryParse(Console.ReadLine(), out num) ? num :
                    throw new BO.BlTryParseFailedException("Parsing failed");
                s_bl.Task.Delete(tmp);
                break;
            case 7:
                CreateSchedule();
                break;
            default:
                throw new BO.BlOptionDoesntExistException("There is no such option in the menu");
        }
    }

    private static void AddTask()
    {
        BO.Task task = new BO.Task() { CreatedAtDate =  DateTime.Now };
        Console.WriteLine("Enter task alias:");
        task.Alias = Console.ReadLine();
        Console.WriteLine("Enter task description:");
        task.Description = Console.ReadLine();
        task.Dependencies = new List<TaskInList>();
        Console.WriteLine("Enter Id of previous tasks the task is dependant on. Enter 0 to stop.");
        int id = int.TryParse(Console.ReadLine(), out int value) ? value : 0;
        while (id != 0)
        {
            task.Dependencies.Add(new BO.TaskInList() { Id = id});
            id = int.TryParse(Console.ReadLine(), out value) ? value : 0;
        }
        Console.WriteLine("Enter task required effort time:");
        task.RequiredEffortTime = TimeSpan.TryParse(Console.ReadLine(), out var time) ? time : null;

        Console.WriteLine("Enter task deliverables:");
        task.Deliverables = Console.ReadLine();
        Console.WriteLine("Enter task remarks:");
        task.Remarks = Console.ReadLine();

        Console.WriteLine("Enter task complexity (0-4):");
        task.Copmlexity = int.TryParse(Console.ReadLine(), out value) ? (BO.EngineerExperience)value : 0;
        Console.WriteLine("Enter task Engineer Id:");
        task.Engineer = int.TryParse(Console.ReadLine(), out value) ? new EngineerInTask() { Id = value} : null;

        s_bl.Task.Create(task);
    }

    private static void UpdateTask()
    {
        Console.WriteLine("Enter the Id of the task you want to update:");
        int id = int.TryParse(Console.ReadLine(), out var num) ? num :
            throw new BlTryParseFailedException("parsing failed");
        BO.Task task = s_bl.Task.Read(id);
        Console.WriteLine("The task you chose is:\n" + task);
        Console.WriteLine("Enter the new data of the task:\n" +
                          "Enter task alias:");
        string? input = Console.ReadLine();
        task.Alias = (input == "") ? task.Alias : input;
        Console.WriteLine("Enter task description:");
        input = Console.ReadLine();
        task.Description = (input == "") ? task.Description : input;

        task.Dependencies = new List<TaskInList>();
        Console.WriteLine("Enter Id of previous tasks the task is dependant on. Enter 0 to stop.");
        int tmp = int.TryParse(Console.ReadLine(), out int value) ? value : 0;
        while (tmp != 0)
        {
            task.Dependencies.Add(new BO.TaskInList() { Id = id });
            id = int.TryParse(Console.ReadLine(), out value) ? value : 0;
        }

        Console.WriteLine("Enter task required effort time:");
        task.RequiredEffortTime = TimeSpan.TryParse(Console.ReadLine(), out var time) ? 
            time : task.RequiredEffortTime;

        Console.WriteLine("Enter task complexity (0-4):");
        task.Copmlexity = int.TryParse(Console.ReadLine(), out value) ? 
            (BO.EngineerExperience)value : task.Copmlexity;

        Console.WriteLine("Enter task Engineer Id:");
        task.Engineer = int.TryParse(Console.ReadLine(), out value) ? 
            new EngineerInTask() { Id = value } : task.Engineer;

        s_bl.Task.Update(task);
    }

    private static void AssignEngineer()
    {
        Console.WriteLine("Enter task Id:");
        int taskId = int.TryParse(Console.ReadLine(), out int num) ? num :
            throw new BO.BlTryParseFailedException("Parsing failed");
        Console.WriteLine("Enter engineer Id:");
        int engineerId = int.TryParse(Console.ReadLine(), out num) ? num :
            throw new BO.BlTryParseFailedException("Parsing failed");
        BO.Task task = s_bl.Task.Read(taskId);
        task.Engineer = new BO.EngineerInTask() { Id = engineerId };
        s_bl.Task.Update(task);
    }

    private static void CreateSchedule() 
    {
        Console.WriteLine("Enter project starting date:");
        DateTime starting = DateTime.TryParse(Console.ReadLine(), out DateTime input) ? input
            : throw new BlTryParseFailedException("parsing failed");
        DateTime tmp;
        var tasks = s_bl.Task.ReadAll(x => x.ScheduledDate == null);
        List<int> unscheduled = new List<int>();
        while (tasks.Count() > 0)
        {
            foreach (var task in tasks)
            {
                    try
                    {
                        Console.WriteLine($"Enter the scheduled date of the task {task.Id}, {task.Alias}.");
                        tmp = DateTime.TryParse(Console.ReadLine(), out input) ? input
                                            : throw new BlTryParseFailedException("parsing failed");
                        s_bl.Task.UpdateTaskDate(task.Id, tmp, starting);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        unscheduled.Add(task.Id);
                    }
                    
                }
            }
        }
    }
}