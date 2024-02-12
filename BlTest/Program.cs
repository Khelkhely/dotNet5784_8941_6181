using System.Security.Cryptography.X509Certificates;
using BO;//?
namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void main(string[] args)
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
                    EngineerMenu();
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
            "3: update a task" +
            "4: assign a task to an engineer" +
            "5: create a schedule for the project");
        int choice = int.TryParse(Console.ReadLine(), out int value) ? value :
                throw new BO.BlTryParseFailedException("Parsing failed");
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
                UpdateTask();
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                throw new BO.BlOptionDoesntExistException("There is no such option in the menu");
        }
    }

    private static void AddTask()
    {
        BO.Task task = new BO.Task() { CreatedAtDate = DateTime.Now };
        Console.WriteLine("Enter task alias:\t");
        task.Alias = Console.ReadLine();
        Console.WriteLine("Enter task description: \t");
        task.Description = Console.ReadLine();
        task.Dependencies = new List<BO.TaskInList>();
        Console.WriteLine("Enter Id of previous tasks the task is dependant on. Enter 0 to stop.\n");
        int id = int.TryParse(Console.ReadLine(), out int value) ? value : 0;
        while (id != 0)
        {
            task.Dependencies.Add(new BO.TaskInList() { Id = id });
            id = int.TryParse(Console.ReadLine(), out value) ? value : 0;
        }

        Console.WriteLine("Enter task scheduled date: \t");
        task.ScheduledDate = DateTime.TryParse(Console.ReadLine(), out var date) ? date : null;
        Console.WriteLine("Enter task start date: \t");
        task.StartDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : null;
        Console.WriteLine("Enter task complete date: \t");
        task.CompleteDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : null;
        Console.WriteLine("Enter task required effort time: \t");
        task.RequiredEffortTime = TimeSpan.TryParse(Console.ReadLine(), out var time) ? time : null;

        Console.WriteLine("Enter task deliverables: \t");
        task.Deliverables = Console.ReadLine();
        Console.WriteLine("Enter task remarks: \t");
        task.Remarks = Console.ReadLine();

        Console.WriteLine("Enter task complexity (0-4): \t");
        task.Copmlexity = int.TryParse(Console.ReadLine(), out value) ? (EngineerExperience)value : 0;
        Console.WriteLine("Enter task Engineer Id: \t");
        task.Engineer = int.TryParse(Console.ReadLine(), out value) ? new EngineerInTask() { Id = value } : null;

        s_bl.Task.Create(task);
    }

    private static void UpdateTask()
    {
        Console.WriteLine("Enter the Id of the task you want to update:\n");
        int id = int.TryParse(Console.ReadLine(), out var num) ? num :
            throw new BlTryParseFailedException("parsing failed");
        BO.Task ogTask = s_bl.Task.Read(id);
        Console.WriteLine("The task you chose is:\n" + ogTask);
        Console.WriteLine("Enter the new data of the task:\n" +
                          "Enter task alias:\t");
        string? input = Console.ReadLine();
        ogTask.Alias = (input == "") ? ogTask.Alias : input;
        Console.WriteLine("Enter task description: \t");
        input = Console.ReadLine();
        ogTask.Description = (input == "") ? ogTask.Description : input;


    }

    private static void EngineerMenu()
    {
        int choice;
        do
        {
            Console.WriteLine("Choose an action:" +
                       "0: return to main menu" +
                       "1: add new engineer" +
                       "2: delete engineer" +
                       "3: show all engineers" +
                       "4: show the tasks that the engineer works on" +
                       "5: update an engineer");
            choice = int.TryParse(Console.ReadLine(), out int value) ? value :
                    throw new BO.BlTryParseFailedException("Parsing failed");
            switch (choice)
            {
                case 1:
                    AddEngineer();
                    break;
                case 2:
                    DeleteEngineer();
                    break;
                case 3:
                    ShowAllEngineers();
                    break;
                case 4:
                    ShowTasks();
                    break;
                case 5:
                    UpdateEngineer();
                    break;
                default:
                    throw new BO.BlOptionDoesntExistException("There is no such option in the menu");
            }
        }
        while (choice != 0);
    }
    private static void AddEngineer()
    {
        Console.WriteLine("Enter engineer's Id: \t");
        if (int.TryParse(Console.ReadLine(), out var id) == false)
            throw new BO.BlTryParseFailedException("parsing failed");
        BO.Engineer engineer = new BO.Engineer() { Id = id };
        Console.WriteLine("Enter engineer's name: \t");
        engineer.Name = Console.ReadLine();
        Console.WriteLine("Enter engineer's email: \t");
        engineer.Email = Console.ReadLine();
        Console.WriteLine("Enter engineer's level (0-4): \t");
        engineer.Level = (BO.EngineerExperience)(int.TryParse(Console.ReadLine(), out int level) ? level : 0);//(int)BO.EngineerExperience.Beginner);
        Console.WriteLine("Enter engineer's cost: \t");
        engineer.Cost = int.TryParse(Console.ReadLine(), out var cost) ? cost : null;       
        try
        {
            s_bl.Engineer.Create(engineer);

        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void DeleteEngineer()
    {
        Console.WriteLine("Enter engineer's Id: \t");
        if (int.TryParse(Console.ReadLine(), out int id) == false)
            throw new BO.BlTryParseFailedException("parsing failed");
        try { s_bl.Engineer.Delete(id); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void ShowAllEngineers()
    {
        foreach (Engineer? item in s_bl.Engineer.ReadAll())
        {
            Console.WriteLine(item);
        }
    }
    private static void ShowTasks()
    {
        Console.WriteLine("Enter engineer's Id: \t");
        if (int.TryParse(Console.ReadLine(), out int id) == false)
            throw new BO.BlTryParseFailedException("parsing failed");
        foreach (BO.Task item in s_bl.Task.ReadAll())
            if (item.Engineer!.Id == id)
                Console.WriteLine(item);
    }
    private static void UpdateEngineer()
    {
        Console.WriteLine("Enter engineer's Id to update: \t");
        if (int.TryParse(Console.ReadLine(), out int id) == false)
            throw new BO.BlTryParseFailedException("parsing failed");
        Engineer original = s_bl.Engineer.Read(id);
        Console.Write(original);

        BO.Engineer engineer = new BO.Engineer() { Id = id };
        Console.WriteLine("Enter engineer's new name: \t");
        engineer.Name = Console.ReadLine();
        Console.WriteLine("Enter engineer's new email: \t");
        engineer.Email = Console.ReadLine();
        Console.WriteLine("Enter engineer's new level (as number): \t");
        engineer.Level = (BO.EngineerExperience)(int.TryParse(Console.ReadLine(), out int level) ? level : 0);
        Console.WriteLine("Enter engineer's cost: \t");
        engineer.Cost = int.TryParse(Console.ReadLine(), out var cost) ? cost : null;
        try { s_bl!.Engineer.Update(engineer); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }


}
