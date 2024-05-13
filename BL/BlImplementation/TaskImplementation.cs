namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

/// <summary>
/// implement task inteface actions 
/// </summary>
internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly Bl _bl;

    internal TaskImplementation(Bl bl) => _bl = bl;

    /// <summary>
    /// add a dependency to task
    /// </summary>
    /// <param name="target">the task another task depends on</param>
    /// <param name="dependOnTask">the task depend on the target task</param>
    /// <exception cref="BlDoesNotExistException">the task chosen to be depend on the target task does not exist</exception>
    public void addDependency(int target, int dependOnTask)
    {
        //check if it is a manager who want create a dependency and throw exception if it is
        if (_bl.GetStatusProject() == StatusProject.Execution)
        {
            throw new BO.BlUnAuthrizedAccessExpetion("Manager cant add a dependency ");
        }
        BO.Task t = _bl.Task.Read(dependOnTask);

        //if there is not task like dependOnTask
        if (t == null)
        {
            throw new BlDoesNotExistException($"task with id = {dependOnTask} does not exist");
        }

        //a loop dependency
        if(_dal.Dependency.ReadByFilter(d=>(d.DependsOnTask == target|| d.DependentTask == dependOnTask)) != null)
        {
            throw new BlLoopDependencyExpetion("a loop dependency is not valid");
        }

        DO.Dependency d = new DO.Dependency() { DependentTask = dependOnTask, DependsOnTask = target };

        _dal.Dependency.Create(d);

    }

    /// <summary>
    /// calculate the status of task. if  a task begin after now it is scheduled. 
    /// if it started but still not finish yet in is ontrack and if it is do not have engineeer to handle it
    /// it is unschedules
    /// </summary>
    /// <param name="task">task to calculate its status</param>
    /// <returns>the task with the updated status</returns>
    private BO.Status CalculateStatus(DO.Task task)
    {
        var dateTimeNow = _bl.Clock;
        
         if ((dateTimeNow < task.BeginWorkDate))
        {
            return BO.Status.Scheduled;
        }

        else if ((dateTimeNow > task.BeginWorkDate) && (dateTimeNow < task.EndWorkTime))
        {
            return Status.OnTrack;
        }
        else if (dateTimeNow > task.EndWorkTime)
        {
            return Status.Done;
        }
        else if (task.EngineerId is null)
        {
            return Status.Unscheduled;
        }
        return Status.Unscheduled;

    }

    /// <summary>
    /// return the engineer handle the task
    /// </summary>
    /// <param name="taskId">id of task to get the engineer</param>
    /// <returns>the engineer handle the task</returns>
    private EngineerInTask getTaskEngineer(int? engineerId)
    {
        if (engineerId != null)
        {
            DO.Engineer e = _dal.Engineer.ReadByFilter(e => e.Id == engineerId);
            return new EngineerInTask { Id = e.Id, Name = e.Name };
        }

        //if task does not have engineer handle it
        return null;
    }

    /// <summary>
    /// get id of task and return the tasks the task depends on them by type of taskInList
    /// </summary>
    /// <param name="taskId">task we wants the tasks it depends on</param>
    /// <returns>the tasks te task dependes on them</returns>
    private List<BO.TaskInList> getTaskDependencies(int taskId)
    {
        // list of all tasks depend on the task
        List<DO.Task?> dependenciesTasks = _dal.Dependency.ReadAll().Where(d =>
        {
            if (d.DependentTask == taskId) { return true; }
            return false;
        }).Select(d => _dal.Task.Read(d.DependsOnTask)).ToList();

        //convert the tasks to taskInList type
        return dependenciesTasks.Where(t => t != null).Select(t => new BO.TaskInList()
        {
            Id = t.Id,
            Alias = t.Name,
            Description = t.TaskDescription,
            StatusTaskInList = CalculateStatus(t)
        }).ToList();

    }


    /// <summary>
    /// calculate the status, dependencies of task, and engineer of task and convert the 
    /// DO type task to BO task by calling the function convertToBoTask in tools class
    /// </summary>
    /// <param name="t">task of DO level type</param>
    /// <returns>task of BO level type</returns>
    private BO.Task convertToBoTask(DO.Task t)
    {
        Status statusTask = CalculateStatus(t);  //calculate the status of the task
        List<BO.TaskInList> dependencies = getTaskDependencies(t.Id); //all tasks depends on this task
        EngineerInTask engineer = getTaskEngineer(t.EngineerId); //engineer handle the task

        //return the task by BO level type
        return Tools.ConvertToBoTask(t, statusTask, dependencies, engineer);
    }


    /// <summary>
    /// create a task type of do level fron task in bo level
    /// </summary>
    /// <param name="item">task to create</param>
    /// <returns>id of the task created</returns>
    /// <exception cref="BO.BlInvalidInputException">name is invalid. cannot create task without name</exception>
    /// <exception cref="BO.BlAlreadyExistsException">can not create a task that already exist</exception>
    public int Create(BO.Task item)
    {
        //check if it is a manager who want create a task and throw exception if it is
        if (_bl.GetStatusProject() == StatusProject.Execution)
        {
            throw new BO.BlUnAuthrizedAccessExpetion("nanager cant create a task ");
        }
        //check all the fields of task user enetered are valid
        if (item.Name == "")
        {
            throw new BO.BlInvalidInputException("invalid name of task");
        }

        //make a task entity of do level
        DO.Task dTask = Tools.ConvertToDoTask(item);

        //check that task does not already exist
        try
        {
            int idTask = _dal.Task.Create(dTask);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={item.Id} already exists", ex);
        }
        catch (Exception ex)
        {
            throw new BlCreateException("failed to create the task ," + ex.Message);
        }
    }

    /// <summary>
    /// delete a task
    /// </summary>
    /// <param name="id">id of task to delete</param>
    /// <exception cref="BlDoesNotExistException">can not delete a task that does not exist</exception>
    /// <exception cref="BlCantDeleteItem">can not delete an item that other tasks depend on it</exception>
    public void Delete(int id)
    {
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException(ex.Message, ex);
        }
        catch (DalDeletionImpossibleException ex)
        {
            throw new BlCannotDeleteItemException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new BlDeleteException($"failed to delete the task with id: {id}," + ex.Message);
        }
    }


    /// <summary>
    /// return bo entity of task by its id. 
    /// </summary>
    /// <param name="id">id of task to read</param>
    /// <returns>bo entity of task with the same id</returns>
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);

        if (doTask == null)//task does not exist
            return null;

        //return the task by BO level type
        return this.convertToBoTask(doTask);

    }

    /// <summary>
    /// return an engineer by filtering. if there is no filter return the first engineer. if there
    /// is a filter return the first match the filter function
    /// </summary>
    /// <param name="filter">filter the engineer list</param>
    /// <returns>the first engineer match the filter or the first in list if there is no filter</returns>
    public BO.Task? ReadByFilter(Func<DO.Task, bool> filter)
    {
        DO.Task t = _dal.Task.ReadByFilter(filter);
        //if there is no task match the filter return null
        if (t == null)
        {
            return null;
        }
        else
        {
            return this.convertToBoTask(t);
        }
    }

    /// <summary>
    /// return all engineers list. if want filter it also filter by a function entered
    /// </summary>
    /// <param name="filter">function to filter the list of engineers</param>
    /// <returns>all list of engineers or filtered list</returns>
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {

        IEnumerable<BO.Task?> result = (from DO.Task doTask in _dal.Task.ReadAll()
                                        select this.convertToBoTask(doTask));

        ///return all tasks exist
        if (filter == null)
        {
            return (from DO.Task doTask in _dal.Task.ReadAll()
                    select this.convertToBoTask(doTask));

        }

        //return only thhe tasks match a filter
        else
        {
            return (from DO.Task doTask in _dal.Task.ReadAll()
                    let BOtask = this.convertToBoTask(doTask)
                    where filter(BOtask)
                    select BOtask
                );
        }

    }

    /// <summary>
    /// update a task. get BO type task and update the database
    /// </summary>
    /// <param name="item">BO type task with fields to update in a task</param>
    /// <exception cref="BO.BlInvalidInputException">cannot update a name of task to empty string</exception>
    /// <exception cref="BlCannotUpdateTaskException">cannot update the task if it does not exist or there are tasks depend on it</exception>
    public void Update(BO.Task item)
    {
        //user must enter a valid id of task to update
        if (item.Id <= 0)
        {
            throw new BO.BlInvalidInputException("invalid id of task");
        }
        //check all the fields of task user enetered are valid
        if (item.Name == "")
        {
            throw new BO.BlInvalidInputException("invalid name of task");
        }
        //check if it is a manager who want update a task

        /*if (_bl.GetStatusProject() == StatusProject.Execution)
        {
            throw new BO.BlCannotUpdateTaskException("manager cannot change or update dates");
        }*/
        //check that the begin date is after the start date of the project
        if (item.BeginWorkDateP != null && (item.BeginWorkDateP < _dal.GetStartDate()))
        {
            throw new BO.BlnotValidDateException("task must begin after the beginning date of project");
        }

        DO.Task doTask = Tools.ConvertToDoTask(item);

        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalCannotUpdateException e)
        {
            throw new BlCannotUpdateTaskException(e.Message, e);
        }
        catch (Exception ex)
        {
            throw new BlDeleteException($"failed to update the task with id: {doTask.Id}," + ex.Message);
        }
    }

    /// <summary>
    /// update a task with date to begin it.
    /// </summary>
    /// <param name="id">id of task to add begin date</param>
    /// <param name="bDateTask">begin date of task</param>
    /// <exception cref="BlInvalidInputException">date is not valid</exception>
    public void UpdateBeginDate(int id, DateTime? bDateTask)
    {


        BO.Task? t = _bl.Task.Read(id);

        if (t is null)
        {
            throw new BlDoesNotExistException($"task with id: {id} does not exist");
        }

        foreach (BO.TaskInList? task in t.Dependencies)
        {
            DateTime? taskBeginDate = _bl.Task.Read(task.Id).BeginWorkDateP;
            //check the date after beginning date of the project, after all dates of tasks its depends on
            if (taskBeginDate == null || bDateTask <= taskBeginDate || bDateTask < _bl.GetStartDate())
            {
                throw new BlInvalidInputException($"not valid begin date of task with id: {id}");
            }
        }

        BO.Task newTask = new BO.Task
        {
            Id = t.Id,
            Name = t.Name,
            EngineerId = t.EngineerId,
            difficulty = t.difficulty,
            StatusTask = t.StatusTask,
            Dependencies = t.Dependencies,
            TaskDescription = t.TaskDescription,
            Product = t.Product,
            Comments = t.Comments,
            CreateTime = t.CreateTime,
            BeginWorkDate = t.BeginWorkDate,
            BeginWorkDateP = bDateTask,
            WorkDuring = t.WorkDuring,
            DeadLine = t.DeadLine,
            EndWorkTime = t.EndWorkTime,
            Engineer = t.Engineer
        };

        _bl.Task.Update(newTask);



    }

    /// <summary>
    /// remove all tasks from db
    /// </summary>
    public void Clear()
    {
        _dal.Task.RemoveAll();
    }
}
