namespace DO;


/// <summary>
/// task engineers need to fullfil
/// </summary>
/// <param name="Id">id of task</param>
/// <param name="Name">name of task</param>
/// <param name="EngineerId">engineer id</param>
/// <param name="difficulty">the difficulty of the task</param>
/// <param name="TaskDescription">description of task</param>
/// <param name="MileStone"></param>
/// <param name="Product">product of the task</param>
/// <param name="Comments"></param>
/// <param name="CreateTime">time of creating the task by manager</param>
/// <param name="BeginWorkDateP">time the task planned to start</param>
/// <param name="BeginWorkDate">time the task started actially</param>
/// <param name="WorkDuring">time of work during</param>
/// <param name="DeadLine">time the task planned to end</param>
/// <param name="EndWorkTime">time the task have been finished actially</param>
public record Task
(
    int Id,
    string Name, 
    int ? EngineerId, 
    EngineerExperience difficulty,   
    string TaskDescription,
    bool MileStone = false,
    string ? Product = null,
    string ? Comments = null, 
    DateTime ? CreateTime= null, 
    DateTime ? BeginWorkDateP = null,  
    DateTime ?  BeginWorkDate = null,  
    int? WorkDuring = null, 
    DateTime ? DeadLine = null, 
    DateTime ? EndWorkTime= null
)
{ 
    public Task() :this(0, "", 0, 0, "") { }//empty ctor
    public override string ToString()//print the value
    {
        return $"Id: {Id}," + "\n "+
               $" Name: {Name}," + "\n " +
               $" EngineerId: {EngineerId}," + "\n " +
               $" Difficulty: {difficulty}," + "\n " +
               $" TaskDescription: {TaskDescription}, " + "\n " +
               $"MileStone: {MileStone}," + " \n" +
               $" Product: {Product}, " + "\n " +
               $"Comments: {Comments}," + "\n " +
               $" CreateTime: {CreateTime}, " + "\n " +
               $"BeginWorkDatePlanned: {BeginWorkDateP}, " + "\n " +
               $"BeginWorkDate: {BeginWorkDate}, " + "\n " +
               $"WorkDuring: {WorkDuring}, " + "\n " +
               $"DeadLine: {DeadLine}, " + "\n " +
               $"EndWorkTime: {EndWorkTime}"+ "\n";
    }
}
