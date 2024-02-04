namespace DalTest;
using Dal;
using DalApi;
using DO;
using System.Diagnostics;

internal class Program
{
    //static readonly IDal s_dal = new DalList(); //stage 2
    //static readonly IDal s_dal = new DalXml(); //stage 3
    static readonly IDal s_dal = Factory.Get; //stage 4

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
        if (int.TryParse(Console.ReadLine(), out choice) == false)
            throw new DalTryParseFailed("parsing failed.");
        while (choice != 0)
        {
            try
            {

                switch (choice)
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
                        Console.Write("Would you like to create Initial data? (Yes/No) "); //stage 3
                        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                        if (ans == "Yes") //stage 3
                        {
                            Initialization.Do(); //stage 2
                        }
                        break;
                    default:
                        throw new DalOptionDoesntExist("There is no such option in the menu.");
                }

            }
            catch (Exception ex) { Console.WriteLine(ex); }
            Console.WriteLine("Choose an entity you want to check:\n0- exit main menu\n1- Tasks\n2- Engineer\n3- Dependency\n4- Initialize data");
            if (int.TryParse(Console.ReadLine(), out choice) == false)
                throw new DalTryParseFailed("parsing failed.");
        }
        
    }

    private static int SubMenuPrint(Objects obj) //print a sub menu for the object received and return the user's choice
    {
        Console.WriteLine("Choose a method you want to perform:");
        Console.WriteLine("0- exit main menu\n1- add a new {0}\n2- show a {0}\n3- show all {0}s\n4- update an existing {0}\n5- delete a {0} from the list", obj);
        int temp;
        if (int.TryParse(Console.ReadLine(), out temp) == false)
            throw new DalTryParseFailed("parsing failed.");
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
                    throw new DalOptionDoesntExist("There is no such option in the menu.");
            }
            x = SubMenuPrint((Objects)0);
        }
    }
    private static void AddTask() //adds a task and prints its ID
    {
        try
        {
            Console.WriteLine(s_dal!.Task.Create(InputTask()));
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static Task InputTask(Task? task = null) //puts task data that is asked from the user into a new task. if the user doesn't enter data, puts in the values from the task that was sent as a parameter.
    {
        if (task == null) //if there wasn't a task sent, puts in default values in areas the user didn't enter anything
            task = new Task(); //a new task with default values
        Console.Write("Enter task alias:    ");
        string? alias = Console.ReadLine();
        if (alias == "") alias = task.Alias;
        Console.Write("Enter task description:    ");
        string? description = Console.ReadLine();
        if (description == "") description = task.Description;
        Console.Write("Enter task creation date:    ");
        DateTime? created, scheduled, started, deadline, complete;
        created = DateTime.TryParse(Console.ReadLine(), out var temp) ? temp : task.CreatedAtDate;
        Console.Write("Enter task scheduled starting date:    "); 
        scheduled = DateTime.TryParse(Console.ReadLine(), out temp) ? temp : task.ScheduledDate;
        Console.Write("Enter task starting date:    ");
        started = DateTime.TryParse(Console.ReadLine(), out temp) ? temp : task.StartDate;
        Console.Write("Enter task required effort time:    ");
        TimeSpan? effort = TimeSpan.TryParse(Console.ReadLine(), out var time) ? time : task.RequiredEffortTime;
        Console.Write("Enter task deadline date:    ");
        deadline = DateTime.TryParse(Console.ReadLine(), out temp) ? temp : task.DeadlineDate;
        Console.Write("Enter task completion date:    ");
        complete = DateTime.TryParse(Console.ReadLine(), out temp) ? temp : task.CompleteDate;
        Console.Write("Enter task deliverables:    ");
        string? deliverables = Console.ReadLine();
        if (deliverables == "") deliverables = task.Deliverables;
        Console.Write("Enter task remarks:    ");
        string? remarks = Console.ReadLine();
        if (remarks == "") remarks = task.Remarks;
        Console.Write("Enter task engineer ID:    ");
        int id = int.TryParse(Console.ReadLine(), out var num) ? num : task.EngineerId;
        Console.Write("Enter task complexity:    ");
        EngineerExperience complexity = StringToEnum(Console.ReadLine()) ?? task.Complexity;
        return new Task(task.Id, alias, description, task.IsMilestone, created, scheduled, started, effort, 
                        deadline, complete, deliverables, remarks, id, complexity);
    }

    private static void ShowTask() //print the task with the Id received from the user
    {
        Console.WriteLine("Enter Task Id:   ");
        int id;
        if (int.TryParse(Console.ReadLine(), out id) == false)
            throw new DalTryParseFailed("parsing failed.");
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
        try
        {
            Console.WriteLine("Enter Task Id:   ");
            int id;
            if (int.TryParse(Console.ReadLine(), out id) == false)
                throw new DalTryParseFailed("parsing failed.");
            Task? original = s_dal!.Task.Read(id);
            Console.WriteLine(original);
            Console.WriteLine("Enter the updated information of the task:");
            Task newTask = InputTask(original);
            s_dal!.Task.Update(newTask);
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void DeleteTask() //removes the task with the received Id from the list of tasks
    {
        Console.WriteLine("Enter Task Id:   ");
        int id;
        if (int.TryParse(Console.ReadLine(), out id) == false)
            throw new DalTryParseFailed("parsing failed.");
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
                    throw new DalOptionDoesntExist("There is no such option in the menu.");
            }
            x = SubMenuPrint((Objects)1);
        }
    }
    private static void AddEngineer() //adds an Engineer and prints his Id
    {
        try 
        {
            Console.WriteLine("Enter engineer ID:   ");
            if (int.TryParse(Console.ReadLine(), out var id) == false)
                throw new DalTryParseFailed("parsing failed");
            Engineer newEngineer = new Engineer(id);
            Console.WriteLine(s_dal!.Engineer.Create(inputEngineer(newEngineer))); 
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void ShowEngineer() //print the Engineer with the Id received from the user
    {
        Console.Write("Enter engineer's ID:   ");
        int id;
        if (int.TryParse(Console.ReadLine(), out id) == false)
            throw new DalTryParseFailed("parsing failed.");
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
        if (int.TryParse(Console.ReadLine(), out id) == false)
            throw new DalTryParseFailed("parsing failed.");
        Engineer? original = s_dal!.Engineer.Read(id);
        Console.Write(original);           
        try { s_dal!.Engineer.Update(inputEngineer(original)); }
        catch (Exception ex) { Console.WriteLine(ex); }

    }
    private static void DeleteEngineer() //removes the engineer with the received Id from the list of engineers
    {
        Console.Write("Enter engineer's ID:   ");
        int id;
        if (int.TryParse(Console.ReadLine(), out id) == false)
            throw new DalTryParseFailed("parsing failed.");
        try { s_dal!.Engineer.Delete(id); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static EngineerExperience? StringToEnum(string? level) //receieves a string with an EngineerExperience and returns the matching enum
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
            case "":
                return null;
            default:
                throw new DalNoSuchEnumExistsException("There is no such engineer's experience.");
        }
    }
    private static Engineer inputEngineer(Engineer? engineer) //puts engineer data that is asked from the user into a new engineer. if the user doesn't enter data, puts in the values from the engineer that was sent as a parameter.
    {
        if (engineer == null) //if there wasn't an engineer sent, puts in default values in areas the user didn't enter anything
            engineer = new Engineer(); //a new engineer with default values
        Console.Write("Enter engineer's name:   ");
        string? name = Console.ReadLine();
        if(name == "") name = engineer.Name;
        Console.Write("Enter engineer's email:   ");
        string? email = Console.ReadLine();
        if(email == "") email = engineer.Email;
        Console.Write("Enter engineer's level:   ");
        EngineerExperience level = StringToEnum(Console.ReadLine()) ?? engineer.Level;
        Console.Write("Enter engineer's cost:   ");
        double? cost = double.TryParse(Console.ReadLine(), out var num) ? num : engineer.Cost;
        Engineer item = new Engineer(engineer.Id, name, email, level, cost);
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
                    throw new DalOptionDoesntExist("There is no such option in the menu.");
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
        if (int.TryParse(Console.ReadLine(), out id) == false)
            throw new DalTryParseFailed("parsing failed.");
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
        if (int.TryParse(Console.ReadLine(), out id) == false)
            throw new DalTryParseFailed("parsing failed.");
        Dependency? original = s_dal!.Dependency.Read(id);
        Console.Write(original);
        try { s_dal!.Dependency.Update(inputDependency(original)!); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static void DeleteDependency()//removes the dependency with the received Id from the list of Dependencies
    {
        Console.Write("Enter dependency's ID:   ");
        int id;
        if (int.TryParse(Console.ReadLine(), out id) == false)
            throw new DalTryParseFailed("parsing failed.");
        try { s_dal!.Dependency.Delete(id); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    private static Dependency inputDependency(Dependency? dependency = null)//puts dependency data that is asked from the user into a new dependency. if the user doesn't enter data, puts in the values from the dependency that was sent as a parameter.
    {
        if (dependency == null) //if there wasn't a dependency sent, puts in default values in areas the user didn't enter anything
            dependency = new Dependency(); //a new dependency with default values
        Console.Write("Enter the number of the dependent task:   ");
        int dependentTask = int.TryParse(Console.ReadLine(), out var num) ? num : dependency.DependentTask;
        Console.Write("Enter the number of the task that it depends on:   ");
        int DependsOnTask = int.TryParse(Console.ReadLine(), out num) ? num : dependency.DependsOnTask;
        Dependency item = new Dependency(dependency.Id, dependentTask, DependsOnTask);
        return item;
    }
}