namespace DalTest;
using Dal;
using DalApi;
using DO;

internal class Program
{
    static readonly IDal s_dal = new DalXml(); //stage 2

    enum Objects
    {
        task,
        engineer,
        dependency
    }

    static void Main(string[] args)
    {
        try 
        {
            MainMenu();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }

    static void MainMenu() //print main menu and send to sub menu according to the input
    {
        Console.WriteLine("Choose an entity you want to check:\n0- exit main menu\n1- Tasks\n2- Engineer\n3- Dependency\n4- Initialize data");
        int choice;
        int.TryParse(Console.ReadLine(), out choice);
        while (choice != 0)
        { 
            switch(choice)
            {
                case 1:
                    TaskSubMenu();
                    break;
                case 2:
                    EngineerSubMenu();
                    break;
                case 3:
                    DependencySubMenu();
                    break;
                case 4:
                    Console.Write("Would you like to create Initial data? (Yes/No)"); //stage 3
                    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                    if (ans == "Yes") //stage 3
                        Initialization.Do(s_dal); //stage 2
                    break;
                default:
                    Console.WriteLine("There is no such option in the main menu.");
                    break;
            }
            Console.WriteLine("Choose an entity you want to check:\n0- exit main menu\n1- Tasks\n2- Engineer\n3- Dependency\n4- Initialize data");
            int.TryParse(Console.ReadLine(), out choice);
        }
        
    }

    private static int SubMenuPrint(Objects obj) //print a sub menu for the object received and return the user's choice
    {
        Console.WriteLine("Choose a method you want to perform:");
        Console.WriteLine($"0- exit main menu\n1- add a new {obj}\n2- show a {obj}\n3- show all {obj}s\n4- update an existing {obj}\n5- delete an {obj} from the list");
        int temp;
        int.TryParse(Console.ReadLine(), out temp);
        return temp;
    }

    private static void TaskSubMenu() //sub menu for Task. calls the function chosen
    {
        int x = SubMenuPrint((Objects)0);
        while (x != 0)
        {
            switch (x)
            {
                case 1:
                    AddTask();
                    break;
                case 2:
                    ShowTask();
                    break;
                case 3:
                    ShowAllTasks();
                    break;
                case 4:
                    UpdateTask();
                    break;
                case 5:
                    DeleteTask();
                    break;
                default:
                    Console.WriteLine("There is no such option in the menu.");
                    break;
            }
            x = SubMenuPrint((Objects)0);
        }
    }
    private static void AddTask() //adds a task and prints its ID
    {
        Console.WriteLine(s_dal!.Task.Create(InputTask()));
    }
    private static Task InputTask() //receives data for a Task and returns a new Task with the data
    {
        Console.Write("Enter task alias:    ");
        string? alias = Console.ReadLine();
        Console.Write("Enter task description:    ");
        string? description = Console.ReadLine();
        Console.Write("Enter task creation date:    ");
        DateTime created, scheduled, started, deadline, complete;//nullble?
        DateTime.TryParse(Console.ReadLine(), out created);
        Console.Write("Enter task scheduled starting date:    ");
        DateTime.TryParse(Console.ReadLine(), out scheduled);
        Console.Write("Enter task starting date:    ");
        DateTime.TryParse(Console.ReadLine(), out started);
        Console.Write("Enter task required effort time:    ");
        int effort;
        int.TryParse(Console.ReadLine(), out effort);
        Console.Write("Enter task deadline date:    ");
        DateTime.TryParse(Console.ReadLine(), out deadline);
        Console.Write("Enter task completion date:    ");
        DateTime.TryParse(Console.ReadLine(), out complete);
        Console.Write("Enter task deliverables:    ");
        string? deliverables = Console.ReadLine();
        Console.Write("Enter task remarks:    ");
        string? remarks = Console.ReadLine();
        Console.Write("Enter task engineer ID:    ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        Console.Write("Enter task complexity:    ");
        string stringComplexity = Console.ReadLine()!;
        EngineerExperience complexity = StringToEnum(stringComplexity);
        return new Task(0, alias, description, false, created, scheduled, started, effort, deadline, complete, deliverables, remarks, id, complexity);
    }

    private static void ShowTask() //print the task with the Id received from the user
    {
        Console.WriteLine("Enter Task Id:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        try
        {
            Console.WriteLine(s_dal!.Task.Read(id));
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void ShowAllTasks() //prints all tasks
    {
        foreach (Task? item in s_dal!.Task.ReadAll())
        {
            Console.Write(item + "\n");
        }
    }
    private static void UpdateTask() //receives an Id and data for a task and if a task with that Id exists updates it to the new data
    {
        Console.WriteLine("Enter Task Id:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        Console.WriteLine(s_dal!.Task.Read(id));
        Console.WriteLine("Enter the updated information of the task:");
        Task newTask = InputTask() with { Id = id };
        try
        {
            s_dal!.Task.Update(newTask);
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void DeleteTask() //removes the task with the received Id from the list of tasks
    {
        Console.WriteLine("Enter Task Id:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        try
        {
            s_dal!.Task.Delete(id);
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }

    private static void EngineerSubMenu() //sub menu for Engineer. calls the function chosen
    {
        int x = SubMenuPrint((Objects)1);
        while (x != 0)
        {
            switch (x)
            {
                case 1:
                    AddEngineer();
                    break;
                case 2:
                    ShowEngineer();
                    break;
                case 3:
                    ShowAllEngineers();
                    break;
                case 4:
                    UpdateEngineer();
                    break;
                case 5:
                    DeleteEngineer();
                    break;
                default:
                    Console.WriteLine("There is no such option in the menu.");
                    break;
            }
            x = SubMenuPrint((Objects)1);
        }
    }
    private static void AddEngineer() //adds an Engineer and prints his Id
    {
        try { Console.WriteLine(s_dal!.Engineer.Create(inputEngineer())); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void ShowEngineer() //print the Engineer with the Id received from the user
    {
        Console.Write("Enter engineer's ID:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        Engineer? myEng = s_dal!.Engineer.Read(id);
        Console.Write(myEng);
    } 
    private static void ShowAllEngineers() //prints all Engineers
    {
        foreach (Engineer? item in s_dal!.Engineer.ReadAll())
        {
            Console.Write(item + "\n");
        }
    }
    private static void UpdateEngineer() //receives an Id and data for an Engineer and if an Engineer with that Id exists updates it to the new data 
    {
        Console.Write("Enter engineer's ID:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        Console.Write(s_dal!.Engineer.Read(id));           
        try { s_dal!.Engineer.Update(inputEngineer()!); }
        catch (Exception ex) { Console.WriteLine(ex); }

    }
    private static void DeleteEngineer() //removes the engineer with the received Id from the list of engineers
    {
        Console.Write("Enter engineer's ID:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        try { s_dal!.Engineer.Delete(id); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static EngineerExperience StringToEnum(string level) //receieves a string with an EngineerExperience and returns the matching enum
    {
        switch (level)
        {
            case "Beginner":
                return (EngineerExperience)0;
            case "AdvancedBeginner":
                return (EngineerExperience)1;
            case "Intermediate":
                return (EngineerExperience)2;
            case "Advanced":
                return (EngineerExperience)3;
            case "Expert":
                return (EngineerExperience)4;
            default:
                throw new DalNoSuchEnumExistsException("There is no such engineer's experience.");
        }
    }
    private static Engineer inputEngineer() //receives data for an Engineer and returns a new Engineer with the data
    {
        Console.Write("Enter engineer's ID:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        Console.Write("Enter engineer's name:   ");
        string? name = Console.ReadLine();
        Console.Write("Enter engineer's email:   ");
        string? email = Console.ReadLine();
        Console.Write("Enter engineer's level:   ");
        string stringLevel = Console.ReadLine()!;
        EngineerExperience level = StringToEnum(stringLevel);
        Console.Write("Enter engineer's cost:   ");
        double cost;
        double.TryParse(Console.ReadLine(), out cost);

        Engineer item = new Engineer(id, name, email, level, cost);
        return item;
    }
    private static void DependencySubMenu()//sub menu for Dependency. calls the function chosen
    {
        int x = SubMenuPrint((Objects)2);
        while (x != 0)
        {
            switch (x)
            {
                case 1:
                    AddDependency();
                    break;
                case 2:
                    ShowDependency();
                    break;
                case 3:
                    ShowAllDependencies();
                    break;
                case 4:
                    UpdateDependency();
                    break;
                case 5:
                    DeleteDependency();
                    break;
                default:
                    Console.WriteLine("There is no such option in the menu.");
                    break;
            }
            x = SubMenuPrint((Objects)2);
        }
    }
    private static void AddDependency()//adds a Dependency and prints his Id
    {
        Console.Write(s_dal!.Dependency.Create(inputDependency()));
    }
    private static void ShowDependency()//print the Dependency with the Id received from the user
    {
        Console.Write("Enter deppendency's ID:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        Dependency? myDep = s_dal!.Dependency.Read(id);
        Console.Write(myDep);
    }
    private static void ShowAllDependencies()//prints all Dependencies
    {
        foreach (Dependency? item in s_dal!.Dependency.ReadAll())
        {
            Console.Write(item + "\n");
        }
    }
    private static void UpdateDependency()//receives an Id and data for a Dependency and if a Dependency with that Id exists updates it to the new data
    {
        Console.Write("Enter dependency's ID:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        Console.Write(s_dal!.Dependency.Read(id));
        try { s_dal!.Dependency.Update(inputDependency()!); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void DeleteDependency()//removes the dependency with the received Id from the list of Dependencies
    {
        Console.Write("Enter dependency's ID:   ");
        int id;
        int.TryParse(Console.ReadLine(), out id);
        try { s_dal!.Dependency.Delete(id); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static Dependency inputDependency()//receives data for a Dependency and returns a new Dependency with the data
    {
        Console.Write("Enter the number of the dependent task:   ");
        int dependentTask;
        int.TryParse(Console.ReadLine(), out dependentTask);

        Console.Write("Enter the number of the task that it depends on:   ");
        int DependsOnTask;
        int.TryParse(Console.ReadLine(), out DependsOnTask);
        Dependency item = new Dependency(dependentTask, DependsOnTask);
        return item;
    }
}