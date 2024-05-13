using System.Threading.Channels;

namespace DO;

/// <summary>
/// engineer need to fullfil the tasks
/// </summary>
/// <param name="Id">id of Engineer</param>
/// <param name="Name">full name of the engineer</param>
/// <param name="EMail">engineer E-Mail</param>
/// <param name="Level">the engineer level</param>
/// <param name="Cost">price per hour</param>
public record Engineer
(
    int Id,
    string Name,
    string EMail,
    EngineerExperience Level,
    double Cost 

)
{ 
    public Engineer() : this(0, "", "", 0, 0) { }//empty ctor
    public override string ToString()//print the item
    {
        return $"Id: {Id}," +"\n"+
            $" Name: {Name}," +"\n"+
            $" EMail: {EMail}," +"\n"+
            $" Level: {Level}," +"\n"+
            $" Cost: {Cost}"+"\n";
    }
}


//class AA
//{
//    Engineer eng = new();
//}