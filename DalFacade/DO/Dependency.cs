

using System.Runtime.CompilerServices;

namespace DO;

/// <summary>
/// dependency between tasks when to start task after taske have been finished
/// </summary>
/// <param name="Id">id of dependency</param>
/// <param name="DependentTask">id of the Dependent Task</param>
/// <param name="DependsOnTask">id of the last task</param>
public record Dependency
(
    int Id, 
    int DependentTask,
    int DependsOnTask
)
{
    public Dependency() : this(0, 0, 0) { }//empty ctor
    public override string ToString()//print the item
    {
        return $"Id: {Id}" +"\n"+
            $", DependentTask:{DependentTask}" + "\n" +
            $" DependsOnTask: {DependsOnTask}";
    }
}
