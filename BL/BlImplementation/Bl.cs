
namespace BlImplementation;
using BlApi;
using BO;

/// <summary>
/// A main class named Bl for the logical layer that implements the main logical interface IBl
/// </summary>
internal class Bl : IBl
{
    #region Clock

    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public void AddHourClock()
    {
      Clock = Clock.AddHours(1);
    }
    public void AddDayClock()
    {
        Clock = Clock.AddDays(1);
    }
    public void InitClock()
    {
        Clock = DateTime.Now;
    }
    #endregion

    //get the beggining date of the project
    public DateTime? GetStartDate() {
        return DalApi.Factory.Get.GetStartDate();
    }

    //set the begginning date of the project
    public void SetStartDate(DateTime? sd) {
        DalApi.Factory.Get.SetStartDate(sd);
    }

    /// <summary>
    /// return the status of the project according to if the beginning project date is being intialized
    /// </summary>
    /// <returns></returns>
    public StatusProject GetStatusProject() {
        if (DalApi.Factory.Get.GetStartDate() == null)
        {
            return StatusProject.Planning;
        }
        //if there are tasks without begin date we in middle status yet
        foreach (DO.Task t in DalApi.Factory.Get.Task.ReadAll())
        {
            if (t.BeginWorkDateP == null)
            {
                return StatusProject.Middle;
            }
        }
        return StatusProject.Execution;
    }

    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.Reset();

    public IEngineer Engineer =>  new EngineerImplementation(this);

    public ITask Task =>  new TaskImplementation(this);
}




