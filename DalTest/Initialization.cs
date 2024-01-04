namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private static ITask? s_dalTask; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static IDependency? s_dalDependency; //stage 1
    private static readonly Random s_rand = new();
    private static void createTasks()
    {
        string[] TaskDescriptions = { };
        string[] TaskDeliverables = { };
        string[] TaskRemarks = { };
        for (int i = 0; i < 20; i++) 
        {
            string? alias = $"T{i}";
            DateTime earliest = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - earliest).Days;
            DateTime created = earliest.AddDays(s_rand.Next(range));
            range = (DateTime.Today - created).Days;
            DateTime scheduled = created.AddDays(s_rand.Next(range));
            range = (DateTime.Today - scheduled).Days;
            DateTime started = scheduled.AddDays(s_rand.Next(range));
            range = (DateTime.Today - started).Days;
            DateTime completed = started.AddDays(s_rand.Next(range));
            range = (DateTime.Today - completed).Days;
            DateTime deadline = completed.AddDays(s_rand.Next(range));
            int required = (deadline - created).Days;
           
            EngineerExperience complexity = (EngineerExperience)(s_rand.Next() % 5);
            int id = 0;
            if (s_dalEngineer!.ReadAll().Count != 0)
            {
                while (s_dalEngineer!.ReadAll().Find(x => x.Level >= complexity) is null)
                    complexity--;
                id = s_dalEngineer!.ReadAll().Find(x => x.Level >= complexity)!.Id;
            }
            Task newTask = new Task(0, alias, TaskDescriptions[i], false, created, scheduled, started, required, deadline, completed, TaskDeliverables[i], TaskRemarks[i], id, complexity);
            s_dalTask!.Create(newTask);
        }
    }
    private static void createDependency()
    {
        if (s_dalTask!.ReadAll().Count != 0)
        {
            for (int i = 0; i < 40; i++)
            {
                int range = s_dalTask!.ReadAll().Count;
                int dependent = s_dalTask!.ReadAll()[s_rand.Next(range)].Id;
                int dependsOn = s_dalTask!.ReadAll()[s_rand.Next(range)].Id;
                while ((dependsOn == dependent) || s_dalDependency!.ReadAll().Exists(x => (x.DependentTask == dependent) && (x.DependsOnTask == dependsOn)))
                    dependsOn = s_dalTask!.ReadAll()[s_rand.Next(range)].Id;
                Dependency newDependency = new Dependency(0, dependent, dependsOn);
                s_dalDependency.Create(newDependency);

            }
        }
        else
            throw new Exception("There are no Tasks to creat Dependency for");
          
    }
    private static void createEngineer()
    {
        string[] EngineerNames = 
        {
            "Rachel Shapiro", "Tehila Cohen", "Yair Cohen", "Dani Levi", "Yosi Klein"
        };
        string[] EngineerEmails =
        {
            "Rachel@gmail.com","Tehila@gmail.com","Yair@gmail.com","Dani@gmail.com","Yosi@gmail.com"
        };
        for (int i = 0;i<5;i++)
        {
            int id;
            do
                id = s_rand.Next(200000000, 400000000);
            while (s_dalEngineer!.Read(id)!=null);
            EngineerExperience level = (EngineerExperience)(s_rand.Next() % 5);
            double cost = s_rand.Next(90,200);
            Engineer newEng = new Engineer(id, EngineerNames[i], EngineerEmails[i], level, cost);
            s_dalEngineer.Create(newEng);
        }
    }
    public static void Do(ITask dalTask, IEngineer dalEngineer, IDependency dalDependency)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        createEngineer();
        createTasks();
        createDependency();
    }
}
