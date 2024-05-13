namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;


/// <summary>
/// implement engineer inteface actions by list
/// </summary>
internal class EngineerImplementation : IEngineer
{
    //create item function
    public int Create(Engineer item)
    {
        if(Read(item.Id) is not null)
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }
    //delete item function
    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"Engineer with Id = {id} is not exist");
        }
        DataSource.Engineers.Remove(Read(id));
    }
    //read the item by id and return the item
    public Engineer? Read(int id)
    {
        Engineer? e = DataSource.Engineers.FirstOrDefault(e => e.Id == id);
        return e;
    
    }
    //return the list of the all items
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        //return only engineers meet a rule
        if (filter != null)
        {
            return (from item in DataSource.Engineers
                   where filter(item)
                   select item);
        }
        //return all the list
        return (from item in DataSource.Engineers
               select item);
    
    }
    //updating the item
    public void Update(Engineer item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Engineer with Id = {item.Id} is not exist");
        }
        Delete(item.Id);
        DataSource.Engineers.Add(item);
    }

    //return first element meet the filter function rule. (not only by id..)
    public Engineer? ReadByFilter(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(e => filter(e));
    }

    //remove all engineers from list
    public void RemoveAll()
    {
        DataSource.Engineers.Clear();
    }
}
