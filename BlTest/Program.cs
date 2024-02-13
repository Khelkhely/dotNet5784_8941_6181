using BO;
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
        bool inProgress = s_bl.IsScheduled(); //a flag to indicate what stage the project is in - plannig, or in progress
        int choice;
        do
        {
            Console.WriteLine("Choose an entity you want to check." +
                "0: Exit main menu" +
                "1: Tasks" +
                "2: Engineer");
            if(!inProgress)
                Console.WriteLine("3: create schedule");
            choice = int.TryParse(Console.ReadLine(), out int value) ? value :
                throw new BlTryParseFailedException("Parsing failed");
            switch (choice)
            {
                case 0:
                    break;
                case 1:
                    TaskMenu();
                    break;
                case 2:
                    EngineerMenu();
                    break;
                case 3:
                    CreateSchedule();
                    break;
                default:
                    throw new BlOptionDoesntExistException("There is no such option in the menu");
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
            "6: delete a task");
        int choice = int.TryParse(Console.ReadLine(), out int value) ? value :
                throw new BlTryParseFailedException("Parsing failed");
        int tmp;
        while (choice != 0)
        {
            try
            {
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
                        Console.WriteLine("Enter task Id:");
                        int taskId = int.TryParse(Console.ReadLine(), out int num) ? num :
                            throw new BlTryParseFailedException("Parsing failed");
                        Console.WriteLine("Enter engineer Id:");
                        int engineerId = int.TryParse(Console.ReadLine(), out num) ? num :
                            throw new BlTryParseFailedException("Parsing failed");
                        s_bl.AssignEngineer(engineerId, taskId);
                        break;
                    case 6:
                        Console.WriteLine("Enter task Id:");
                        tmp = int.TryParse(Console.ReadLine(), out num) ? num :
                            throw new BO.BlTryParseFailedException("Parsing failed");
                        s_bl.Task.Delete(tmp);
                        break;
                    default:
                        throw new BO.BlOptionDoesntExistException("There is no such option in the menu");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Choose an action:" +
            "0: return to main menu" +
            "1: add new task" +
            "2: show all tasks" +
            "3: show task" +
            "4: update a task" +
            "5: assign a task to an engineer" +
            "6: delete a task");
            choice = int.TryParse(Console.ReadLine(), out value) ? value :
                throw new BO.BlTryParseFailedException("Parsing failed");
        }

    }
    /*private static void TaskMenuAfter()
    {
        Console.WriteLine("Choose an action:" +
            "0: return to main menu" +
            "1: show all tasks" +
            "2: show task" +
            "3: update a task" +
            "4: assign a task to an engineer");
        int choice = int.TryParse(Console.ReadLine(), out int value) ? value :
                throw new BO.BlTryParseFailedException("Parsing failed");
        int tmp;
        while (choice != 0)
        {
            switch (choice)
            {
                case 1:
                    foreach (var item in s_bl.Task.ReadAll())
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case 2:
                    Console.WriteLine("Enter task Id:");
                    tmp = int.TryParse(Console.ReadLine(), out int num) ? num :
                        throw new BO.BlTryParseFailedException("Parsing failed");
                    Console.WriteLine(s_bl.Task.Read(tmp));
                    break;
                case 3:
                    UpdateTask();
                    break;
                case 4:
                    AssignEngineer();
                    break;
                case 5:
                default:
                    throw new BO.BlOptionDoesntExistException("There is no such option in the menu");
            }
            Console.WriteLine("Choose an action:" +
            "0: return to main menu" +
            "1: add new task" +
            "2: show all tasks" +
            "3: show task" +
            "4: update a task" +
            "5: assign a task to an engineer" +
            "6: delete a task");
            choice = int.TryParse(Console.ReadLine(), out value) ? value :
                throw new BO.BlTryParseFailedException("Parsing failed");
        }

    }*/
    private static void AddTask()
    {
        BO.Task task = new BO.Task();
        Console.WriteLine("Enter task alias:");
        task.Alias = Console.ReadLine();
        Console.WriteLine("Enter task description:");
        task.Description = Console.ReadLine();
        task.Dependencies = new List<TaskInList>();
        Console.WriteLine("Enter Id of previous tasks the task is dependant on. Enter 0 to stop.");
        int id = int.TryParse(Console.ReadLine(), out int value) ? value : 0;
        while (id != 0)
        {
            task.Dependencies.Add(new TaskInList() { Id = id });
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
        task.Engineer = int.TryParse(Console.ReadLine(), out value) ? new EngineerInTask() { Id = value } : null;

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
            task.Dependencies.Add(new TaskInList() { Id = id });
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
                    throw new BlTryParseFailedException("Parsing failed");
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
                    throw new BlOptionDoesntExistException("There is no such option in the menu");
            }
        }
        while (choice != 0);
    }
    private static void AddEngineer()
    {
        Console.WriteLine("Enter engineer's Id: \t");
        if (int.TryParse(Console.ReadLine(), out var id) == false)
            throw new BlTryParseFailedException("parsing failed");
        BO.Engineer engineer = new Engineer() { Id = id };
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
            throw new BlTryParseFailedException("parsing failed");
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
            throw new BlTryParseFailedException("parsing failed");
        foreach (BO.Task item in s_bl.Task.ReadAll())
            if (item.Engineer!.Id == id)
                Console.WriteLine(item);
    }
    private static void UpdateEngineer()
    {
        Console.WriteLine("Enter engineer's Id to update: \t");
        if (int.TryParse(Console.ReadLine(), out int id) == false)
            throw new BlTryParseFailedException("parsing failed");
        Engineer original = s_bl.Engineer.Read(id);
        Console.Write(original);

        BO.Engineer engineer = new Engineer() { Id = id };
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


    private static void CreateSchedule()
    {
        Console.WriteLine("Enter project starting date:");
        DateTime starting = DateTime.TryParse(Console.ReadLine(), out DateTime input) ? input
            : throw new BlTryParseFailedException("parsing failed");

        int choice, taskId;
        DateTime date;
        do
        {
            Console.WriteLine("Choose an action: " +
            "0: exit to main menu" +
            "1: schedule a task" +
            "2: finish scheduling");
            choice = int.TryParse(Console.ReadLine(), out int num) ? num :
                throw new BlTryParseFailedException("Parsing failed");
            switch (choice)
            {
                case 0:
                    break;
                case 1:
                    try
                    {
                        Console.WriteLine("Enter task Id:");
                        taskId = int.TryParse(Console.ReadLine(), out num) ? num :
                            throw new BlTryParseFailedException("Parsing failed");
                        Console.WriteLine("Enter task scheduled date:");
                        date = DateTime.TryParse(Console.ReadLine(), out var tmp) ? tmp :
                            throw new BlTryParseFailedException("Parsing failed");
                        s_bl.Task.UpdateTaskDate(taskId, date, starting);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case 2:
                    try
                    {
                        s_bl.CreateSchedule(starting);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                default:
                    throw new BlOptionDoesntExistException("There is no such option in the menu");
            }
        }
        while (choice != 0);
    }
}