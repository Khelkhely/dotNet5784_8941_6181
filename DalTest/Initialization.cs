namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private static IDal? s_dal;//stage2
 
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
        string[] TaskDeliverables =
        {
            "Work plan",
            "Layer 1 interface",
            "layer 1 implementation",
            "A main program for testing",
            "List of problems to fix",
            "Finished layer 1",
            "Documentation for layer 1",
            "Layer 2 interface",
            "layer 2 implementation",
            "A main program for testing",
            "List of problems to fix",
            "Finished layer 2",
            "Documentation for layer 2",
            "Layer 3 interface",
            "layer 3 implementation",
            "A main program for testing",
            "List of problems to fix",
            "Finished layer 3",
            "Documentation for layer 3",
            "Finished project"
        };
        string[] TaskRemarks = {
            "use agile method",
            "Bring more cookies next time",
            "the temperature is too high",
            "buy a new air conditioner",
            "fix the new air conditioner",
            "buy magnum fridge",
            "lock the fridge after leaving work",
            "switch the pairs because they always fight",
            "don't forget to drink water",
            "remind others to drink water too",
            "it's very important to keep hydrated",
            "buy bug repelent",
            "Rachel and Tehila are very good",
            "we should give them a raise",
            "they put day and night into this project",
            "call an engineer that can fix the air conditioning",
            "cheer up all the employees",
            "give the employees their salary",
            "don't forget to pay the instelator",
            "invite everyone to a party for finishing the project"};
        
        for (int i = 0; i < 20; i++) 
        {
            string? alias = $"T{i}";
            DateTime earliest = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - earliest).Days;
            DateTime created = earliest.AddDays(s_rand.Next(range)); //chooses a random date from 2020 until today
           
            EngineerExperience complexity = (EngineerExperience)(s_rand.Next() % 5); //choose a random numer from 0-4 and convert to EngineerExperience
            Task newTask = new Task(0, alias, TaskDescriptions[i], false, created, null, null, null, null, null, TaskDeliverables[i], TaskRemarks[i], 0, complexity);
            s_dal!.Task.Create(newTask);
        }
    }
    private static void createDependency()
    {
        //for each task number i, the id is i +1000, so added 1000 to each task in the dependency
        for (int i = 1; i < 20; i++) //all tasks depend on the first task because it plans the work
        {
            Dependency newDependency = new Dependency(0, i + 1000, 0 + 1000);
            s_dal!.Dependency.Create(newDependency);
        }
        for (int i = 0; i < 19; i++) //the last task depends on all of the previos ones because its checking the entire project
        {
            Dependency newDependency = new Dependency(0, 19 + 1000, i + 1000);
            s_dal!.Dependency.Create(newDependency);
        }
        for (int i = 4; i < 19; i += 6) //every testing of a layer depends on the completion of the layer
        {
            for (int j = 1; j < 4; j++)
            {
                Dependency newDependency = new Dependency(0, i + 1000, i - j + 1000);
                s_dal!.Dependency.Create(newDependency);
            }
        }
        for (int i = 4; i < 19; i += 6) //the documentation and the bug fixing depend on the testing of each layer
        {
            Dependency newDependency = new Dependency(0, i + 1 + 1000, i + 1000);
            s_dal!.Dependency.Create(newDependency);
            newDependency = new Dependency(0, i + 2 + 1000, i + 1000);
            s_dal!.Dependency.Create(newDependency);
        }
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
                id = s_rand.Next(200000000, 400000000); //creates a random 9 digit id that isn't already used
            while (s_dal!.Engineer.Read(id)!=null);
            EngineerExperience level = (EngineerExperience)(s_rand.Next() % 5); //choose a random numer from 0-4 and convert to EngineerExperience
            double cost = s_rand.Next(90,200); //choose a random amount of money for the salary between 90 and 200
            Engineer newEng = new Engineer(id, EngineerNames[i], EngineerEmails[i], level, cost);
            s_dal!.Engineer.Create(newEng);
        }
    }
    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!");

        createEngineer();
        createTasks();
        createDependency();
    }
}
