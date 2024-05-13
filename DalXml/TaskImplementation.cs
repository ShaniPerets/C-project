using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dal;
using DalApi;
using DO;

namespace Dal;

internal class TaskImplementation : ITask
{
    //direct path to file tasks.xml:
    private const string _s_tasks = "tasks";

    //create new engineer  by using the xml files
    public int Create(DO.Task item)
    {
        IEngineer eCruds = new EngineerImplementation();

        //check that the engineer id is valid
        DO.Engineer e = eCruds.ReadByFilter((e) => { return e.Id == item.EngineerId; });

        if (e == null && item.EngineerId != null)
        {
            throw new DalDoesNotExistException($"Engineeer with id: { item.EngineerId } does not exist");
        }

        List<DO.Task?> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(_s_tasks);
        int id = Config.NextTaskId;
        DO.Task copy = item with { Id = id };
        tasks.Add(copy);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, _s_tasks);
        return id;
    }

    //delete engineer by using the xml files
    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"Task with Id = {id} is not exist");
        }

        List<DO.Task?> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task?>(_s_tasks);
        
        
        IDependency dCruds = new DependencyImplementation();

        //if there is another task depends on this task- cannot delete this task
        List<DO.Dependency> dependesOnTaskList = dCruds.ReadAll(d => d.DependsOnTask == id).ToList();
        
        /*if (dependesOnTaskList.Count > 0)
        {
            throw new DalDeletionImpossibleException($"cannot delete task that another task depends on in dependency id: {dependesOnTaskList.ElementAt(0).Id}");
        }*/

        //delete all dependencies with the task is there as a dependent task.
        List<DO.Dependency> dependencies = dCruds.ReadAll(d=>d.DependentTask == id).ToList();
        foreach (DO.Dependency d in dependencies)
        {
            dCruds.Delete(d.Id);
        }

        tasks.Remove(Read(id));
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, _s_tasks);
    }

    //return a task by its id. null if does not find
    public DO.Task? Read(int id)
    {
        List<DO.Task?> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task?>(_s_tasks);
        DO.Task? t = tasks.Where(t => t.Id == id).FirstOrDefault();
        return t;
    }

    //return a task matches a function's rule called filter. null if does not find
    public DO.Task? ReadByFilter(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(_s_tasks);

        return tasks.FirstOrDefault(t => filter(t));

    }
    //returns all the tasks  by using the xml files

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(_s_tasks);

        //return only tasks meet a rule
        if (filter != null)
        {
            return (from item in tasks
                    where filter(item)
                    select item);
        }
        //return all the list
        return (from item in tasks
                select item);
    }

    private int CreateForUpdate(DO.Task item)
    {
        IEngineer eCruds = new EngineerImplementation();

        //check that the engineer id is valid
        DO.Engineer e = eCruds.ReadByFilter((e) => { return e.Id == item.EngineerId; });

        if (e == null && item.EngineerId != null)
        {
            throw new DalDoesNotExistException($"Engineeer with id: {item.EngineerId} does not exist");
        }

        List<DO.Task?> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(_s_tasks);
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, _s_tasks);
        return item.Id;
    }

    //uptade the task details  by using the xml files

    public void Update(DO.Task item)
    {
        try
        {
            Delete(item.Id);
            CreateForUpdate(item);
        }
        catch (DalDeletionImpossible ex)
        {
            throw new DalCannotUpdateException($"can not update task with id = {item.Id} which has tasks depend on it");
        }
        catch(DalDoesNotExistException ex){
            throw new DalCannotUpdateException($"can not update task {item.Id} which not exist");
        }
        
    }

    /// <summary>
    /// remove all tasks from database
    /// </summary>
    public void RemoveAll()
    {
        List<DO.Task?> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(_s_tasks);
        tasks.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, _s_tasks);

        //restart the id in config to 1
        XElement configData = XMLTools.LoadListFromXMLElement("data-config");
        configData.Element("NextTaskId").Value = "1";
        XMLTools.SaveListToXMLElement(configData, "data-config");

    }
}


