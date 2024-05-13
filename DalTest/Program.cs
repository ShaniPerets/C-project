using Dal;
using DalApi;
using DalTest;

using DO;

internal class Program
{
    //factory will implement the _s_dal to the way we want: xml/list etc according to config.
    static readonly IDal _s_dal = Factory.Get;
    

    static void Main(string[] args)
    {
        try
        {

            //ask a user if want to initializa the database
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y") //initialize the database
            {
                Initialization.Do();
            }

            //open menu with entities to choose for 
            chooseEntities();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    //menu for choose an entitiy
    private static void chooseEntities()
    {
        int entity = 1;
        while (entity != 0) //while user didnt choose exit:
        {
            Console.WriteLine("choose an entity you want to check:");
            Console.WriteLine(" 1 - task \n 2 - engineer \n 3 - dependency \n 0 - to exit");
            entity = int.Parse(Console.ReadLine());
            switch (entity)
            {
                //exit
                case 0:
                    break;
                //task
                case 1:
                    try
                    {
                        taskCrud("task");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                //engineer
                case 2:
                    try
                    {
                        engineerCrud("engineer");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                //dependency
                case 3:
                    try
                    {
                        dependencyCrud("dependency");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                //error- not valid choice
                default:
                    {
                        throw new DalNotValidNumber($"{entity} is not a valid number");
                    }
            }
        }
    }


    //show task cruds to choose
    private static void taskCrud(string entity)
    {
        Console.WriteLine($"choose a method you want to check for entity: {entity}");
        Console.WriteLine(" 0 - exit \n 1 - Create \n 2 - Read \n 3 - ReadAll \n 4 - Update \n 5-delete");
        int chosen = int.Parse(Console.ReadLine());
        switch (chosen)
        {
            //exit
            case 0:
                break;
            //create a task
            case 1:
                DO.Task t = makeTask();
                _s_dal!.Task.Create(t);
                break;
            //read a task by its id
            case 2:
                Console.WriteLine("enter id");
                int idR = int.Parse(Console.ReadLine());
                DO.Task taskR = _s_dal!.Task.Read(idR);
                if (taskR is null)
                {
                    throw new DalDoesNotExistException("there is no such task");
                }
                //print task 
                Console.WriteLine(taskR);
                break;
            //read all the tasks exist
            case 3:
                List<DO.Task?> listTasks = (List<DO.Task?>)(_s_dal!.Task.ReadAll().ToList());
                foreach (DO.Task ta in listTasks)
                {
                    if (ta is not null)
                        Console.WriteLine(ta); ;
                }
                break;
            //update a task
            case 4:
                Console.WriteLine("please enter id of task to update: ");
                int id = int.Parse(Console.ReadLine());
                DO.Task taskU = _s_dal!.Task.Read(id);
                if (taskU is null)
                {
                    throw new DalDoesNotExistException("task to update does not exist");
                }
                Console.WriteLine(taskU);
                Console.WriteLine("enter values to update: ");
                DO.Task updatedTask = newUpdatedTask(taskU);
                _s_dal!.Task.Update(updatedTask);
                break;
            //delete a task 
            case 5:
                Console.WriteLine("enter id of task to delete");
                int idD = int.Parse(Console.ReadLine());
                _s_dal!.Task.Delete(idD);
                break;
            default:
                {
                    throw new DalNotValidNumber($" {entity} is not a valid number");
                }
        }
    }

    //show engineer crud to choose
    private static void engineerCrud(string entity)
    {
        Console.WriteLine($"choose a method you want to check for entity: {entity}");
        Console.WriteLine(" 0 - exit \n 1 - Create \n 2 - Read \n 3 - ReadAll \n 4 - Update \n 5-delete");
        int chosen = int.Parse(Console.ReadLine());
        switch (chosen)
        {
            //exit
            case 0:
                break;
            //create an engineer
            case 1:
                Engineer e = makeEngineer();
                _s_dal!.Engineer.Create(e);
                break;
            //read angineer
            case 2:
                Console.WriteLine("enter id");
                int idR = int.Parse(Console.ReadLine());
                DO.Engineer engineer = _s_dal!.Engineer.Read(idR);
                if (engineer is null)
                {
                    throw new DalDoesNotExistException("there is not such engineer");
                }
                Console.WriteLine(_s_dal!.Engineer.Read(idR));
                break;
            //show all the engineers
            case 3:
                List<Engineer?> listEngineer = (List<DO.Engineer?>)(_s_dal!.Engineer.ReadAll().ToList());
                foreach (Engineer en in listEngineer)
                {
                    if (en is not null)
                        Console.WriteLine(en);
                }
                break;
            //update an engineer
            case 4:
                Console.WriteLine("please enter id of engineer to update: ");
                int id = int.Parse(Console.ReadLine());
                DO.Engineer engineerU = _s_dal!.Engineer.Read(id);
                if (engineerU is null)
                {
                    throw new DalDoesNotExistException("engineer to update does not exist");
                }
                Console.WriteLine(engineerU);
                Console.WriteLine("enter values to update: ");
                DO.Engineer eU = newUpdatedEngineer(engineerU);
                _s_dal!.Engineer.Update(eU);
                break;
            //delete an engineer
            case 5:
                Console.WriteLine("enter id of engineer to delete");
                int idD = int.Parse(Console.ReadLine());
                _s_dal!.Engineer.Delete(idD);
                break;
            default:
                {
                    throw new DalNotValidNumber($" {entity} is not a valid number");
                }
        }
    }

    //show dependency crud to choose
    private static void dependencyCrud(string entity)
    {
        Console.WriteLine($"choose a method you want to check for entity: {entity}");
        Console.WriteLine(" 0 - exit \n 1 - Create \n 2 - Read \n 3 - ReadAll \n 4 - Update \n 5-delete");
        int chosen = int.Parse(Console.ReadLine());
        switch (chosen)
        {
            case 0:
                break;
            //create a dependency
            case 1:
                Dependency d = makeDependency();
                _s_dal!.Dependency.Create(d);
                break;
            //show a specific dependency
            case 2:
                Console.WriteLine("enter id");
                int idR = int.Parse(Console.ReadLine());
                DO.Dependency dependency = _s_dal!.Dependency.Read(idR);
                if (dependency is null)
                {
                    throw new DalDoesNotExistException("there is not such dependency");
                }
                Console.WriteLine(dependency);
                break;
            //show all dependencies
            case 3:
                List<Dependency?> listDependecy = (List<DO.Dependency?>)(_s_dal!.Dependency.ReadAll().ToList());
                foreach (Dependency de in listDependecy)
                {
                    if (de is not null)
                        Console.WriteLine(de);
                }
                break;
            //update a dependency
            case 4:
                Console.WriteLine("please enter id of dependency to update: ");
                int id = int.Parse(Console.ReadLine());
                DO.Dependency dependencyU = _s_dal!.Dependency.Read(id);
                if (dependencyU is null)
                {
                    throw new DalDoesNotExistException("engineer to update does not exist");
                }
                Console.WriteLine(dependencyU);
                Console.WriteLine("enter values to update: ");
                Dependency dU = newUpdatedDependency(dependencyU);
                _s_dal!.Dependency.Update(dU);
                break;
            //delete a dependency
            case 5:
                Console.WriteLine("enter id of dependnecy to delete");
                int idD = int.Parse(Console.ReadLine());
                _s_dal!.Dependency.Delete(idD);
                break;
            default:
                {
                    throw new DalNotValidNumber($" {entity} is not a valid number");
                }
        }
    }

    //make a task object
    private static DO.Task makeTask()
    {
        Console.WriteLine("enter name,difficulty  between 0-4,taskDescription ,Product,Comments,CreateTime,BeginWorkDateP,BeginWorkDate,WorkDuring,DeadLine,EndWorkTime");
        string name = Console.ReadLine();
        int ?engineerId = null;
        int difficulty = int.Parse(Console.ReadLine());
        string taskDescription = Console.ReadLine();
        string? product = Console.ReadLine();
        string? comments = Console.ReadLine();
        if (comments == "") { comments = null; }
        if (product == "") { product = null; }
        string createTime = Console.ReadLine();
        string beginWorkDateP = Console.ReadLine();
        string beginWorkDate = Console.ReadLine();
        string workDuring = Console.ReadLine();
        string deadLine = Console.ReadLine();
        string endWorkTime = Console.ReadLine();
        DateTime? CreateTime = (createTime == "") ? null : DateTime.Parse(createTime);
        DateTime? BeginWorkDateP = (beginWorkDateP == "") ? null : DateTime.Parse(beginWorkDateP);
        DateTime? BeginWorkDate = (beginWorkDate == "") ? null : DateTime.Parse(beginWorkDate);
        int? WorkDuring = (workDuring == "") ? null : int.Parse(workDuring);
        DateTime? DeadLine = (deadLine == "") ? null : DateTime.Parse(deadLine);
        DateTime? EndWorkTime = (endWorkTime == "") ? null : DateTime.Parse(endWorkTime);
        EngineerExperience experience = (EngineerExperience)difficulty;
        DO.Task t = new DO.Task(0, name, engineerId, experience, taskDescription, false, product, comments, CreateTime, BeginWorkDateP, BeginWorkDate, WorkDuring, DeadLine, EndWorkTime);
        return t;
    }

    //make an engineer object
    private static DO.Engineer makeEngineer()
    {
        Console.WriteLine("enter id, name,Email,level between 0-4 and cost");
        int id = int.Parse(Console.ReadLine());
        string name = Console.ReadLine();
        string Email = Console.ReadLine();
        int level = int.Parse(Console.ReadLine());
        EngineerExperience experience = (EngineerExperience)level;
        double cost = double.Parse(Console.ReadLine());
        Engineer e = new Engineer(id, name, Email, experience, cost);
        return e;
    }

    //make a dependency objetc
    private static DO.Dependency makeDependency()
    {
        Console.WriteLine("DependentTask and DependsOnTask");
        int id1 = int.Parse(Console.ReadLine());
        //check the task exist:
        DO.Task task1 = _s_dal!.Task.Read(id1);
        if (task1 is null)
        {
            throw new DalDoesNotExistException("task does not exist for dependency");
        }
        int id2 = int.Parse(Console.ReadLine());
        //check the task exist
        DO.Task task2 = _s_dal!.Task.Read(id2);
        if (task2 is null)
        {
            throw new DalDoesNotExistException("task does not exist for dependency");
        }
       
        Dependency d = new Dependency(0, id1, id2);
        return d;
    }

    //update a task. id values are null: dont update th value.
    private static DO.Task newUpdatedTask(DO.Task taskU)
    {
        Console.WriteLine("enter name,engineer id,difficulty  between 0-4 ,MileStone,Product,Comments,CreateTime,BeginWorkDateP,BeginWorkDate,WorkDuring,DeadLine,EndWorkTime");
        int id = taskU.Id;
        string name = Console.ReadLine();
        if (name == "")
        {
            name = taskU.Name;
        }
        int? engineerId = int.Parse(Console.ReadLine());
        if (engineerId == 0) { engineerId = taskU.EngineerId; };
      
        string difficult = Console.ReadLine();
        EngineerExperience difficulty;
        if (difficult == "") { difficulty = taskU.difficulty; }
        else { difficulty = (EngineerExperience)int.Parse(Console.ReadLine()); }
        string taskDescription = Console.ReadLine();
        if (taskDescription == "") { taskDescription = taskU.TaskDescription; }
        string product = Console.ReadLine();
        if (product == "") { product = taskU.Product; }
        string comments = Console.ReadLine();
        if (comments == "") { comments = taskU.Comments; }
        string createTime = Console.ReadLine();
        string beginWorkDateP = Console.ReadLine();
        string beginWorkDate = Console.ReadLine();
        string workDuring = Console.ReadLine();
        string deadLine = Console.ReadLine();
        string endWorkTime = Console.ReadLine();
        DateTime? CreateTime = (createTime == "") ? taskU.CreateTime : DateTime.Parse(createTime);
        DateTime? BeginWorkDateP = (beginWorkDateP == "") ? taskU.BeginWorkDateP : DateTime.Parse(beginWorkDateP);
        DateTime? BeginWorkDate = (beginWorkDate == "") ? taskU.BeginWorkDate : DateTime.Parse(beginWorkDate);
        int? WorkDuring = (workDuring == "") ? taskU.WorkDuring : int.Parse(workDuring);
        DateTime? DeadLine = (deadLine == "") ? taskU.DeadLine : DateTime.Parse(deadLine);
        DateTime? EndWorkTime = (endWorkTime == "") ? taskU.EndWorkTime : DateTime.Parse(endWorkTime);
        DO.Task t = new DO.Task(id, name, engineerId, difficulty, taskDescription, false, product, comments, CreateTime, BeginWorkDateP, BeginWorkDate, WorkDuring, DeadLine, EndWorkTime);
        return t;
    }

    //update a ENGINEER. id values are null: dont update th value.
    private static DO.Engineer newUpdatedEngineer(DO.Engineer engineerU)
    {
        Console.WriteLine("name,Email,level between 0-4 and cost");
        string name = Console.ReadLine();
        if (name == "") { name = engineerU.Name; }
        string Email = Console.ReadLine();
        if (Email == "") { Email = engineerU.EMail; }
        string level = Console.ReadLine();
        EngineerExperience experience;
        if (level == "") { experience = engineerU.Level; }
        else { experience = (EngineerExperience)(int.Parse(level)); }
        double costU;
        string cost = Console.ReadLine();
        if (cost == "") { costU = engineerU.Cost; }
        else { costU = double.Parse(cost); }
        Engineer eU = new Engineer(engineerU.Id, name, Email, experience, costU);
        return eU;
    }

    //make a dependency objetc
    private static DO.Dependency newUpdatedDependency(Dependency dependency)
    {
        Console.WriteLine("DependentTask and DependsOnTask");
        string id = Console.ReadLine();
        int id1 = dependency.DependentTask;
        int id2 = dependency.DependsOnTask;
        //update task 1
        if (id != "")
        {
            id1 = int.Parse(id);
            //check the task exist:
            DO.Task task1 = _s_dal!.Task.Read(id1);
            if (task1 is null)
            {
                throw new DalDoesNotExistException("task does not exist for dependency");
            }
            DateTime? deadline1 = task1.DeadLine;
            id = Console.ReadLine();
            //update task 2
            if (id != "")
            {
                id2 = int.Parse(id);
                //check the task exist
                DO.Task task2 = _s_dal.Task.Read(id2);
                if (task2 is null)
                {
                    throw new DalDoesNotExistException("task does not exist for dependency");
                }
    
            }

        }

        Dependency d = new Dependency(0, id1, id2);
        return dependency;
    }

}



