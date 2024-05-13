
namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

/// <summary>
/// implement engineer inteface actions 
/// </summary>
internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly Bl _bl;

    internal EngineerImplementation(Bl bl) => _bl = bl;

    /// <summary>
    /// get the engineer id and return task the engineer responsible for it. null if there is no task to the engineer
    /// </summary>
    /// <param name="EngineerId">id of engineer we want his tasks</param>
    /// <returns>task the engineer responsible for it</returns>
    public BO.TaskInEngineer GetTheEngineerTasks(int EngineerId)
    {
        DO.Task engineerTask = _dal.Task.ReadByFilter((t) => { return t.EngineerId == EngineerId; });

        //engineer does not have tasks
        if (engineerTask == null)
        {
            return null;
        }
        BO.TaskInEngineer t = new TaskInEngineer() { Id = engineerTask.Id, Alias = engineerTask.Name };
        return t;
    }

    /// <summary>
    /// get the task the engineer handle and convert the 
    /// DO type enginer to BO engineer by calling the function convertToBoEngineer in tools class
    /// </summary>
    /// <param name="eng">engineer of DO type</param>
    /// <returns>engineer of BO level type</returns>
    private BO.Engineer convertToBoEngineer(DO.Engineer eng)
    {
        TaskInEngineer t = GetTheEngineerTasks(eng.Id);
        return Tools.ConvertToBoEngineer(eng, t);
    }

    /// <summary>
    /// check if email is valid email
    /// </summary>
    /// <param name="email">email string to check it is valid email</param>
    /// <returns>true if the email is valid. false if it is not</returns>
    private static bool IsValidEMail(string email)
    {
        bool valid = true;

        try
        {
            var emailAddress = new MailAddress(email);
        }
        catch //email is not valid
        {
            valid = false;
        }

        return valid;
    }

    /// <summary>
    /// add an engineer to the do level from the user input. throw exception if the input not valid
    /// </summary>
    /// <param name="boEngineer">engineer user inserted from bo level</param>
    /// <returns>engineer item of do level</returns>
    /// <exception cref="BO.Exceptions.BlInvalidInputException">invalid input of user exception</exception>
    /// <exception cref="BO.BlAlreadyExistsException">the user alredy exist exception</exception>
    /// <exception cref="BO.BlDoesNotExistException">engineer id is not valid. there is not such engineer</exception>
    public int Create(BO.Engineer item)
    {
        //check all the fields of engineer user enetered are valid
        if (item.Id <= 0)
        {
            throw new BO.BlInvalidInputException("invalid id of engineer");
        }
        if (item.Name == "")
        {
            throw new BO.BlInvalidInputException("invalid name of engineer");
        }
        if (item.Cost <= 0)
        {
            throw new BO.BlInvalidInputException("invalid cost of engineer. cost must be positive number");
        }
        if (!IsValidEMail(item.EMail))
        {
            throw new BO.BlInvalidInputException("invalid mail address");
        }


        //make a engineer entity of do level
        DO.Engineer doEngineer = Tools.ConvertToDoEngineer(item);

        //check that engineer does not already exist
        try
        {
            int idEng = _dal.Engineer.Create(doEngineer);
            return idEng;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={item.Id} already exists", ex);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new BlCreateException("failed to create the engineer ," + ex.Message);
        }
    }

    /// <summary>
    /// delete an engineer by id, checks if there a task to complete
    /// </summary>
    /// <param name="id">id of engineer to delete</param>
    /// <exception cref="BlDoesNotExistException">there is no such engineer</exception>
    /// <exception cref="BO.BlCannotDeleteItemException">engineer that has a task to fulfill that is not done cannot be deleted</exception>
    public void Delete(int id)
    {

        BO.Engineer eng = Read(id);
        if (eng == null)
        {
            throw new BlDoesNotExistException($"Engineer with ID={id} doesnt exist");
        }
        if (eng.Task != null)
        {
            BO.Task t = _bl.Task.Read(eng.Task.Id);

            //if the engineer has a task to finish - task that its satus is not done yet - we cannot delete him
            if (t != null && (int)t.StatusTask != 4)
            {
                throw new BO.BlCannotDeleteItemException($"cant delete engineer in a middle of a task");
            }
        }

        if (_bl.GetStatusProject() == StatusProject.Execution)
        {
            List<BO.Task> tasks = _bl.Task.ReadAll().ToList();
            foreach (BO.Task task in tasks)
            {
                if (task.EngineerId == id)
                {
                    throw new BlCannotDeleteItemException("cannot delete engineer that have a task to fulfill");
                }
            }
        }
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }
        catch (Exception ex)
        {
            throw new BlDeleteException($"failed to delete the engineer with id: {id}," + ex.Message);
        }


    }


    /// <summary>
    /// return bo entity of engineer by its id. 
    /// </summary>
    /// <param name="id">id of engineer to read</param>
    /// <returns>bo entity of engineer with the same id</returns>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);

        if (doEngineer == null)//engineer does not exist
            return null;

        return this.convertToBoEngineer(doEngineer);
    }

    /// <summary>
    /// return an engineer by filtering. if there is no filter return the first engineer. if there
    /// is a filter return the first match the filter function
    /// </summary>
    /// <param name="filter">filter the engineer list</param>
    /// <returns>the first engineer match the filter or the first in list if there is no filter</returns>
    public BO.Engineer? ReadByFilter(Func<DO.Engineer, bool> filter)
    {
        DO.Engineer eng = _dal.Engineer.ReadByFilter(filter);

        //if there is no engineer mtch the filter return null
        if (eng == null)
        {
            return null;
        }
        else
        {
            return convertToBoEngineer(eng);
        }
    }

    /// <summary>
    /// return all engineers list. if want filter it also filter by a function entered
    /// </summary>
    /// <param name="filter">function to filter the list of engineers</param>
    /// <returns>all list of engineers or filtered list</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {

        //return all the engineers exist
        if (filter == null)
        {
            return (from DO.Engineer doEng in _dal.Engineer.ReadAll()
                    select convertToBoEngineer(doEng));
        }

        //return only engineers match the filter
        else
        {
            return (from DO.Engineer doEng in _dal.Engineer.ReadAll()
                    let BOeng = convertToBoEngineer(doEng)
                    where filter(BOeng)
                    select BOeng
                );
        }

    }
    /// <summary>
    /// update the item
    /// </summary>
    /// <param name="item">engineer to update. fileds are the new fields to update</param>
    /// <exception cref="BO.BlInvalidInputException">id or cost is negative or name of engineer is empty string</exception>

    public void Update(BO.Engineer item)
    {
        //check all the fields of engineer user enetered are valid
        if (item.Id <= 0)
        {
            throw new BO.BlInvalidInputException("invalid id of engineer");
        }
        if (item.Name == "")
        {
            throw new BO.BlInvalidInputException("invalid name of engineer");
        }
        if (item.Cost <= 0)
        {
            throw new BO.BlInvalidInputException("invalid cost of engineer. cost must be positive number");
        }
        if (!IsValidEMail(item.EMail))
        {
            throw new BO.BlInvalidInputException("invalid mail address");
        }

        //task the engineer has to fulfill
        int? taskId = item.Task?.Id;

        DO.Engineer doEngineer = Tools.ConvertToDoEngineer(item);
        try
        {
            _dal.Engineer.Update(doEngineer);
            //update the task of the engineer
            if (taskId is not null)
            {
                BO.Task t = _bl.Task.Read(taskId ?? 0);
                t.EngineerId = item.Id;
                _bl.Task.Update(t);

            }


        }
        catch (BO.BlnotValidDateException ex)
        {
            throw new BO.BlnotValidDateException("failed to update task of engineer", ex);
        }
        catch (BO.BlCannotUpdateTaskException ex)
        {
            throw new BO.BlnotValidDateException("failed to update task of engineer", ex);
        }
        catch (DO.DalDoesNotExistException e)
        {
            throw new BlCannotUpdateTaskException(e.Message, e);
        }
        catch (Exception ex)
        {
            throw new BlUpdateException($"failed to update the egineer with id: {item.Id}," + ex.Message);
        }

    }

    /// <summary>
    /// remove all engineers from db
    /// </summary>
    public void Clear()
    {
        _dal.Engineer.RemoveAll();
    }
}

