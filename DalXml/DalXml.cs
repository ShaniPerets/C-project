using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
namespace Dal;

/// <summary>
/// We have implemented within the DalXml class for each class the appropriate attribute defined for it in the IDal interface, so that they return objects of the type that implements the appropriate interface for each data entity
/// </summary>
sealed internal class DalXml : IDal
{
    //make the class dalxml singelton:
    public static IDal Instance { get; } = new DalXml();

    private DalXml() { } //empty private constructor

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public void SetStartDate(DateTime? sd) //set the beginning date of the project
    {
        DateTime start = sd ?? DateTime.Now;
        XMLTools.SetStartDate("data-config", "StartDate", start);
    }

    public DateTime ?  GetStartDate() //return the beginning date of the project
    {
        return XMLTools.GetStartDate("data-config", "StartDate");
    }
}
