using BO;
using System.Security.Cryptography.X509Certificates;

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
        BO.Task task = new BO.Task() { CreatedAtDate =  DateTime.Now };
        Console.WriteLine("Enter task alias:\t");
        task.Alias = Console.ReadLine();
        Console.WriteLine("Enter task description: \t");
        task.Description = Console.ReadLine();
        task.Dependencies = new List<TaskInList>();
        Console.WriteLine("Enter Id of previous tasks the task is dependant on. Enter 0 to stop.\n");
        int id = int.TryParse(Console.ReadLine(), out int value) ? value : 0;
        while (id != 0)
        {
            task.Dependencies.Add(new BO.TaskInList() { Id = id});
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
        task.Engineer = int.TryParse(Console.ReadLine(), out value) ? new EngineerInTask() { Id = value} : null;

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
}