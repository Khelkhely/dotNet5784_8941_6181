namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private static ITask? s_dalTask; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static IDependency? s_dalDependency; //stage 1
    private static readonly Random s_rand = new();

    private static void createDpendency()
    {
        int range = s_dalTask!.ReadAll().Count;
        int dependent = s_dalTask!.ReadAll()[s_rand.Next(range)].Id;
        int dependsOn = s_dalTask!.ReadAll()[s_rand.Next(range)].Id;
        
        Dependency newDependency = new Dependency(0, dependent, dependsOn);

    }
}
