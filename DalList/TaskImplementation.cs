namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// implement task inteface actions by list
/// </summary>
internal class TaskImplementation : ITask
{
    //create item function
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }
    //delete item function
    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"Task with Id = {id} is not exist");
        }
        DataSource.Tasks.Remove(Read(id));
    }
    //read the item by id and return the item
    public Task? Read(int id)
    {
        Task? t = DataSource.Tasks.FirstOrDefault(t => t.Id == id);
        return t;
       
    }
    //return the list of the all items
    public IEnumerable<Task> ReadAll(Func <Task, bool>? filter = null)
    {
        //return only tasks meet a rule
        if (filter != null)
        {
            return (from item in DataSource.Tasks
                   where filter(item)
                   select item);
        }
        //return all the list
        return (from item in DataSource.Tasks
               select item);
      
    }
    //updating the item
    public void Update(Task item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Task with Id = {item.Id} is not exist");
        }
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }

    //return first element meet the filter function rule. (not only by id..)
    public Task? ReadByFilter(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(t => filter(t));
    }

    //remove all tasks from list
    public void RemoveAll()
    {
        DataSource.Tasks.Clear();
    }
}
