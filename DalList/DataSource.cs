
namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// createing the lists of each one
    /// </summary>
    internal static List<DO.Dependency?> Dependencys { get; } = new();
    internal static List<DO.Engineer?> Engineers { get; } = new();
    internal static List<DO.Task?> Tasks { get; } = new();

    //update the run primary keys
    internal static class Config
    {
        internal static DateTime ? beginProjectDate = null; //the beginning date of the project
        internal const int StartTaskId = 1;
        private static int _nextTaskId = StartTaskId;
        internal static int NextTaskId  { get => _nextTaskId++; }
        internal const int StartDependencyId = 100;
        private static int _nextDependencyId = StartTaskId;
        internal static int NextDependencyId  { get => _nextDependencyId++; }
    }
}
