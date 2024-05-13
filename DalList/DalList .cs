using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Dal;
using DalApi;
/// <summary>
/// implement the interfaces of dal level. initialize any interface by its implementation (in this case list type)
/// </summary>
sealed internal class DalList : IDal
{
    //make calss DalList a singelton:
    public static IDal Instance { get; } = new DalList();
    private DalList() { }  //private empty constructor

    public IDependency Dependency =>  new DependencyImplementation();

    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();

    public void SetStartDate(DateTime? sd) { DataSource.Config.beginProjectDate = sd; }

    public DateTime? GetStartDate() { return DataSource.Config.beginProjectDate; }

}

