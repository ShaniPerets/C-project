
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// implement dependency inteface actions by list
/// </summary>
internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencys.Add(copy);
        return id;
    }
    //delete item function
    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"Task with Id = {id} is not exist");
        }
        DataSource.Dependencys.Remove(Read(id));
    }

    //read the item by id and return the item
    public Dependency? Read(int id)
    {
        Dependency? d = DataSource.Dependencys.FirstOrDefault(d => d.Id == id);
        return d;
    }

    //return the list of the all items
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        //return only dependencies meet a some rule
        if (filter != null)
        {
            return (from item in DataSource.Dependencys
                    where filter(item)
                    select item);
        }
        //return all list
        return (from item in DataSource.Dependencys
               select item);

    }

    //updating the item
    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Task with Id = {item.Id} is not exist");
        }
        Delete(item.Id);
        DataSource.Dependencys.Add(item);
    }

    //return first element meet the filter function rule. (not only by id..)
    public Dependency? ReadByFilter(Func<Dependency, bool> filter)
    {

        return DataSource.Dependencys.FirstOrDefault(d => filter(d));
    }

    //remove all dependences from list
    public void RemoveAll()
    {
        DataSource.Dependencys.Clear();
    }

}
