using DO;

namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        internal const int startTaskId = 1000;
        public static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
        
        internal const int startDependencyId = 1000;
        public static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
        internal static DateTime? projectStartDate { get; set; } = null;
        internal static DateTime? projectEndDate { get; set; } = null;
    }

    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();

}
