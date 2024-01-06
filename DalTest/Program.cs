using DO;
using DalApi;//?
using Dal;//?
namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");







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
        private static EngineerExperience StringToEnum(string level)
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
        private static Engineer inputEngineer()
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
        private static Dependency inputDependency()
        {
            Console.Write("Enter the number of the dependent task:   ");
            int dependentTask = Console.Read();
            Console.Write("Enter the number of the task that it depends on:   ");
            int DependsOnTask = Console.Read();
            Dependency item = new Dependency(dependentTask, DependsOnTask);
            return item;
        }


    }
}
