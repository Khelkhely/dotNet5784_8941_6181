using DO;
using System.ComponentModel.Design;
using System.Xml.Serialization;

namespace DalTest
{
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IEngineer? s_dalEngineer = new EngineerImlementation(); //stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1

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
                Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
                MainMenu();

            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        private void MainMenu()
        {
            Console.WriteLine("Choose an entity you want to check:\n0- exit main menu\n1- Tasks\n2- Engineer\n3- Dependency");
            int choice = Console.Read();
            while(choice != 0)
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
                    default:
                        Console.WriteLine("")
                }
                choice = Console.Read();
            }
            
        }

        private int SubMenuPrint(Objects obj)
        {
            Console.WriteLine("Choose a method you want to perform:");
            Console.WriteLine($"0- exit main menu\n1- add a new {obj}\n2- show a {obj}\n3- show all {obj}s\n4- update an existing {obj}\n5- delete an {obj} from the list");
            return Console.Read();
        }

        private void TaskSubMenu() 
        {
            int x = SubMenuPrint(task);
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
                        PrintAllTAsks();
                        break;
                    case 4:
                        UpadteTask();
                        break;
                    case 5:
                        DeleteTask();
                        break;
                    default:
                        Console.WriteLine("There is no such option in the menu.");
                }
                x = Console.Read();
            }
        }
        private void AddTask()
        {
            Console.WriteLine(s_dalTask!.Create(InputTask()));
        }
        private Task InputTask() 
        {
            Console.Write("Enter task alias:    ");
            string? alias = Console.ReadLine();
            Console.Write("Enter task description:    ");
            string? description = Console.ReadLine();
            Console.Write("Enter task creation date:    ");
            string? tmp = Console.ReadLine();
            DateTime? created, scheduled, started, deadline, complete;
            TryParse(tmp, created);
            Console.Write("Enter task scheduled starting date:    ");
            tmp = Console.ReadLine();
            TryParse(tmp, scheduled);
            Console.Write("Enter task starting date:    ");
            tmp = Console.ReadLine();
            TryParse(tmp, started);
            Console.Write("Enter task required effort time:    ");
            int? effort = Console.Read();
            Console.Write("Enter task deadline date:    ");
            tmp = Console.ReadLine();
            TryParse(tmp, deadline);
            Console.Write("Enter task completion date:    ");
            tmp = Console.ReadLine();
            TryParse(tmp, complete);
            Console.Write("Enter task deliverables:    ");
            string? deliverables = Console.ReadLine();
            Console.Write("Enter task remarks:    ");
            string? remarks = Console.ReadLine();
            Console.Write("Enter task engineer ID:    ");
            int id? = Console.Read();
            Console.Write("Enter task complexity:    ");
            EngineerExperience complexity = Console.Read();
            return new Task(0, alias, description, false, created, scheduled, started, effort, deadline, complete, deliverables, remarks, id, complexity);
        }
        private void ShowTask()
        {
            Console.WriteLine("Enter Task Id:   ");
            int id = Console.Read();
            try
            {
                Console.WriteLine(s_dalTask!.Read(id));
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private void PrintAllTAsks()
        {
            Console.WriteLine(s_dalTask!.ReadAll());
        }
        private void UpadteTask()
        {
            Console.WriteLine("Enter Task Id:   ");
            int id = Console.Read();
            Console.WriteLine(s_dalTask!.Read());
            Console.WriteLine("Enter the updated information of the task:");
            Task newTask = InputTask() with { Id = id }
            try
            {
                s_dalTask!.Update(newTask);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private void DeleteTask()
        {
            Console.WriteLine("Enter Task Id:   ");
            int id = Console.Read();
            try
            {
                s_dalTask!.Delete(id);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        private void EngineerSubMenu()
        {
            int x = SubMenuPrint(engineer);
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
                }
                x = Console.Read();
            }
        }
        private void AddEngineer()
        {
            try { Console.WriteLine("The engineer ID is " + s_dalEngineer!.Create(inputEngineer())); }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private void ShowEngineer()
        {
            Console.Write("Enter engineer's ID:   ");
            int id = Console.Read();
            Engineer? myEng = s_dalEngineer!.Read(id);
            Console.Write(myEng);
        }
        private void ShowAllEngineers()
        {
            Console.Write(s_dalEngineer!.ReadAll());
        }
        private void UpdateEngineer()
        {
            Console.Write("Enter engineer's ID:   ");
            int id = Console.Read();
            Console.Write(s_dalEngineer!.Read(id));           
            try { s_dalEngineer.Update(inputEngineer()!); }
            catch (Exception ex) { Console.WriteLine(ex); }

        }
        private void DeleteEngineer()
        {
            Console.Write("Enter engineer's ID:   ");
            int id = Console.Read();
            try { s_dalEngineer!.Delete(id); }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private EngineerExperience StringToEnum(string level)
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
                    throw new Exception("There is no such engineer's experience.");
            }
        }
        private Engineer inputEngineer()
        {
            Console.Write("Enter engineer's ID:   ");
            int id = Console.Read();
            Console.Write("Enter engineer's name:   ");
            string? name = Console.ReadLine();
            Console.Write("Enter engineer's email:   ");
            string? email = Console.ReadLine();
            Console.Write("Enter engineer's level:   ");
            string stringLevel = Console.ReadLine()!;
            EngineerExperience level = StringToEnum(stringLevel);
            Console.Write("Enter engineer's cost:   ");
            double? cost = Console.Read();
            Engineer item = new Engineer(id, name, email, level, cost);
            return item;
        }

        private void DependencySubMenu()
        {
            int x = SubMenuPrint(dependency);
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
                }
                x = Console.Read();
            }
        }
        private void AddDependency()
        {
            Console.Write("The dependency's ID is " + s_dalDependency!.Create(inputDependency()));
        }
        private void ShowDependency()
        {
            Console.Write("Enter deppendency's ID:   ");
            int id = Console.Read();
            Dependency? myDep = s_dalDependency!.Read(id);
            Console.Write(myDep);
        }
        private void ShowAllDependencies()
        {
            Console.Write(s_dalDependency!.ReadAll());
        }
        private void UpdateDependency()
        {
            Console.Write("Enter dependency's ID:   ");
            int id = Console.Read();
            Console.Write(s_dalDependency!.Read(id));
            try { s_dalDependency.Update(inputDependency()!); }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private void DeleteDependency()
        {
            Console.Write("Enter dependency's ID:   ");
            int id = Console.Read();
            try { s_dalDependency!.Delete(id); }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private Dependency inputDependency()
        {
            Console.Write("Enter the number of the dependent task:   ");
            int dependentTask = Console.Read();
            Console.Write("Enter the number of the task that it depends on:   ");
            int DependsOnTask = Console.Read();
            Dependency item = new Dependency(0, dependentTask, DependsOnTask);
            return item;
        }
    }

 
}
