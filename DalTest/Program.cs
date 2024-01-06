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

            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        void MainMenu()
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
        private void DependencySubMenu()
        {

        }
    }

 
}
