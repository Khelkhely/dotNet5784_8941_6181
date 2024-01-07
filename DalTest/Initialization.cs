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
        string[] TaskDescriptions =
        {
            "Planning and distribution of tasks between teams",
            "Writing interfaces for layer 1",
            "Implement the Layer 1 interfaces",
            "Writing a main program for layer 1 testing",
            "Layer 1 testing",
            "Fix bugs in layer 1",
            "Writing documentation for layer 1",
            "Writing interfaces for layer 2",
            "Implement the Layer 2 interfaces",
            "Writing a main program for layer 2 testing",
            "Layer 2 testing",
            "Fix bugs in layer 2",
            "Writing documentation for layer 2",
            "Writing interfaces for layer 3",
            "Implement the Layer 3 interfaces",
            "Writing a main program for layer 3 testing",
            "Layer 3 testing",
            "Fix bugs in layer 3",
            "Writing documentation for layer 3",
            "Testing all layers together"
        };
        string[] TaskDeliverables = { };
        string[] TaskRemarks = { };
        for (int i = 0; i < 20; i++) 
        {
            string? alias = $"T{i}";
            DateTime earliest = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - earliest).Days;
            DateTime created = earliest.AddDays(s_rand.Next(range));
           
            EngineerExperience complexity = (EngineerExperience)(s_rand.Next() % 5);
            Task newTask = new Task(0, alias, TaskDescriptions[i], false, created, null, null, null, null, null, TaskDeliverables[i], TaskRemarks[i], 0, complexity);
            s_dalTask!.Create(newTask);
        }
    }
    private static void createDependency()
    {
        if (s_dalTask!.ReadAll().Count != 0)
        {
            for (int i = 1; i < 20; i++) 
            {
                Dependency newDependency = new Dependency(0, i, 0);
                s_dalDependency!.Create(newDependency);
            }
            for (int i = 0; i < 19; i++)
            {
                Dependency newDependency = new Dependency(0, 19, i);
                s_dalDependency!.Create(newDependency);
            }
            for(int i=4;i<19;i+=6)
            {
                for(int j=1;j<4;j++)
                {
                    Dependency newDependency = new Dependency(0, i, i-j);
                    s_dalDependency!.Create(newDependency);
                }
            }
            for (int i=4;i<19;i+=6)
            {
                Dependency newDependency = new Dependency(0, i+1, i);
                s_dalDependency!.Create(newDependency);
                newDependency = new Dependency(0, i + 2, i);
                s_dalDependency!.Create(newDependency);
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
