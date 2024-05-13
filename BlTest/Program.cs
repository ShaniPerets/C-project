using BO;
using BlApi;
using DO;
using System.ComponentModel;
using DalApi;

internal class Program
{
    private static readonly Random s_rand = new();

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// check and try all cruds of entities task and engineer in bo
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="FormatException">invalid input, unwanted format</exception>
    static void Main(string[] args)
    { 
        try
        {
            //ask a user if want to initializa the database
            string ans;
            Console.Write("Would you like to create Initial data? (Y/N)");
            ans = Console.ReadLine(); 
            while(ans != "Y" && ans != "N") {
                Console.WriteLine("wrong input. please try again");
                ans = Console.ReadLine();
            }
            if (ans == "Y") //initialize the database
            {
                DalTest.Initialization.Do();
            }

            bool running = true;
            while (running)
            {
                Console.WriteLine("do you want to run the program? y/n");
                string ans2=Console.ReadLine();
                while (ans2 != "y" && ans2 != "n")
                {
                    Console.WriteLine("wrong input. please try again");
                    ans = Console.ReadLine();
                }
                if (ans2 == "y")
                {
                    running = true;
                }
                else
                {
                    running =false;  
                }
                if (running == true)
                {
                    if (s_bl.GetStatusProject() == StatusProject.Middle)
                    {
                        initTasksBeginDates();
                    }
                    else
                    {
                        chooseEntities();
                    }
                    
                }
            }
            
            

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private static void startProject()
    {
        Console.WriteLine(s_bl.GetStartDate());
    }

    //menu for choose an entitiy
    private static void chooseEntities()
    {
        int entity = 1;
        while (entity != 0) //while user didnt choose exit:
        {
            Console.WriteLine("choose an entity you want to check:");
            Console.WriteLine(" 1 - task \n 2 - engineer \n3- start the project \n0 - to exit");
            entity = int.Parse(Console.ReadLine());
            while(entity < 0 || entity>3)
            {
                Console.WriteLine("wrong input. please try again");
                entity = int.Parse( Console.ReadLine());
            }
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
                case 3:
                    Console.WriteLine("add a beginning date to the project"); 
                    DateTime bDateProject;
                    bool isSucceed = DateTime.TryParse(Console.ReadLine(), out bDateProject);
                    if (!isSucceed)
                    {
                        throw new BlInvalidInputException("not valid date to begin project");
                    }
                    s_bl.SetStartDate(bDateProject); //set the beginning date of the project
                    initTasksBeginDates();
                    entity = 0; //get out of choose entities. - there is no option to change the entities like planning status
                    break;
                //error- not valid choice
                default:
                    {
                        throw new BlInvalidInputException($"{entity} is not a valid number");
                    }
            }
        }
    }

    //make it update the tasks it dependeds on before.. and check the dates beteween task dependencies
    private static void initTasksBeginDates()
    {

        List<BO.Task> tasks = s_bl.Task.ReadAll().ToList();

        //check all tasks are initialized with begginng dates
        bool stop = (s_bl.GetStatusProject() != StatusProject.Middle);


        //all tasks have begin dates- return
        if (stop)
        {
            Console.WriteLine("all tasks are initialized with beginning dates!");
            return;
        }

        //have tasks not initialized yet
        Console.WriteLine("do you want to initilaize date of tasks automatically? y/n");
        string ans = Console.ReadLine();
        while (ans != "y" && ans != "n")
        {
            Console.WriteLine("wrong input. please try again");
            ans = Console.ReadLine();
        }
        //initialize the dates automatically
        if (ans == "y")
        {
            automateBeginDatesTasks();
            while ((s_bl.GetStatusProject() == StatusProject.Middle))
            {
                automateBeginDatesTasks();
            }
        }

        stop = (s_bl.GetStatusProject() != StatusProject.Middle);


        if (!stop)

            //ask the user to enter begin dates to all tasks until they all have it.
            while (!stop)
            {
                Console.WriteLine("add id of task and date to begin it");
                int id;
                bool isSucceed = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceed)
                {
                    throw new BlInvalidInputException("id must be a number");
                }
                DateTime bDateTask;
                isSucceed = DateTime.TryParse(Console.ReadLine(), out bDateTask);
                if (!isSucceed)
                {
                    throw new BlInvalidInputException("not valid date to begin task");
                }
                try
                {
                    s_bl.Task.UpdateBeginDate(id, bDateTask);
                }
                catch (BlInvalidInputException e)
                {
                    throw new BlInvalidInputException(e.Message);
                }
                catch (BlCannotUpdateTaskException e)
                {
                    throw new BlCannotUpdateTaskException(e.Message);
                }
                catch (BlUpdateException e)
                {
                    throw new BlUpdateException(e.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //check all tasks are initialized with beginning date
                stop = (s_bl.GetStatusProject() != StatusProject.Middle);

            }

    }

    //init begin dates to all tasks automatically
    private static void automateBeginDatesTasks()
    {
        DateTime today = DateTime.Now;

        foreach (BO.Task t in s_bl.Task.ReadAll().ToList())
        {
            if(s_bl.GetStatusProject() != StatusProject.Middle)
            {
                break;
            }
            List<BO.TaskInList?> tasksFirst = t.Dependencies;
            bool canUpdate = true;
            DateTime start = s_bl.GetStartDate() ?? DateTime.MinValue; //the beginning date of the project
            if (t.Dependencies is not null)
            {
                foreach (BO.TaskInList? tF in tasksFirst)
                {
                    DateTime? beginDateT = s_bl.Task.Read(tF.Id).BeginWorkDateP;
                    if (beginDateT is null)
                    {
                        canUpdate = false;
                        break;
                    }
                    if (beginDateT > start) //make sure the date we update is after all the dependecies tasks
                    {
                        start = beginDateT ?? start;
                    }
                }
            }
            if (canUpdate == false)
            {
                continue;
            }
            //range is the number of days range that createTime might be later then start date.
            int range = (today - start).Days;
            DateTime? bDateTask = start.AddDays(s_rand.Next(1+range));
            try
            {
                s_bl.Task.UpdateBeginDate(t.Id, bDateTask);
            }
            catch(Exception ex)
            {
                Console.WriteLine( ex.Message);
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
                try
                {
                    BO.Task t = makeTask();
                    int tId = s_bl!.Task.Create(t);
                    addDependencies(tId);
                }
                catch(Exception ex )
                {
                    Console.WriteLine( ex.Message);
                }
                break;
            //read a task by its id
            case 2:
                try
                {
                    Console.WriteLine("enter id");
                    int idR = int.Parse(Console.ReadLine());
                    BO.Task taskR = s_bl!.Task.Read(idR);
                    if (taskR is null)
                    {
                        throw new BlDoesNotExistException("there is no such task");
                    }
                    //print task 
                    Console.WriteLine(taskR);
                }
                catch (Exception ex)
                {
                    Console.WriteLine( ex.Message);
                }
                break;
            //read all the tasks exist
            case 3:
                try
                {
                    List<BO.Task?> listTasks = (List<BO.Task?>)(s_bl!.Task.ReadAll().ToList());
                    foreach (BO.Task ta in listTasks)
                    {
                        if (ta is not null)
                            Console.WriteLine(ta); ;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            //update a task
            case 4:
                try
                {
                    Console.WriteLine("please enter id of task to update: ");
                    int id = int.Parse(Console.ReadLine());
                    BO.Task taskU = s_bl!.Task.Read(id);
                    if (taskU is null)
                    {
                        throw new BlDoesNotExistException("task to update does not exist");
                    }
                    Console.WriteLine(taskU);
                    Console.WriteLine("enter values to update: ");
                    BO.Task updatedTask = newUpdatedTask(taskU);
                    s_bl!.Task.Update(updatedTask);
                }
                catch (Exception e)
                {
                    Console.WriteLine( e.Message);
                }
                break;
            //delete a task 
            case 5:
                try
                {
                    Console.WriteLine("enter id of task to delete");
                    int idD = int.Parse(Console.ReadLine());
                    s_bl!.Task.Delete(idD);
                }
                catch(Exception ex) {
                    Console.WriteLine( ex.Message);
                }
                break;
            default:
                {
                    throw new BlInvalidInputException($" {entity} is not a valid number");
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
                try
                {
                    BO.Engineer e = makeEngineer();
                    s_bl!.Engineer.Create(e);
                }
                catch( Exception ex ) {
                    Console.WriteLine( ex.Message);
                }
                break;
            //read angineer
            case 2:
                try
                {
                    Console.WriteLine("enter id");
                    int idR = int.Parse(Console.ReadLine());
                    BO.Engineer engineer = s_bl!.Engineer.Read(idR);
                    if (engineer is null)
                    {
                        throw new BlDoesNotExistException("there is not such engineer");
                    }
                    Console.WriteLine(engineer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            //show all the engineers
            case 3:
                try
                {
                    List<BO.Engineer?> listEngineer = (List<BO.Engineer?>)(s_bl!.Engineer.ReadAll().ToList());
                    foreach (BO.Engineer? en in listEngineer)
                    {
                        if (en is not null)
                            Console.WriteLine(en);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            //update an engineer
            case 4:
                try
                {
                    Console.WriteLine("please enter id of engineer to update: ");
                    int id = int.Parse(Console.ReadLine());
                    BO.Engineer engineerU = s_bl!.Engineer.Read(id);
                    if (engineerU is null)
                    {
                        throw new BlDoesNotExistException("engineer to update does not exist");
                    }
                    Console.WriteLine(engineerU);
                    Console.WriteLine("enter values to update: ");
                    BO.Engineer eU = newUpdatedEngineer(engineerU);
                    s_bl!.Engineer.Update(eU);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            //delete an engineer
            case 5:
                try
                {
                    Console.WriteLine("enter id of engineer to delete");
                    int idD = int.Parse(Console.ReadLine());
                    s_bl!.Engineer.Delete(idD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            default:
                {
                    throw new BlInvalidInputException($" {entity} is not a valid number");
                }
        }
    }

        //make a task object
        private static BO.Task makeTask()
    {
        Console.WriteLine("enter name, difficulty  between 0-4,taskDescription ,Product,Comments,WorkDuring");
        string name = Console.ReadLine();
        int difficulty = int.Parse(Console.ReadLine());
        string taskDescription = Console.ReadLine();
        string? product = Console.ReadLine();
        string? comments = Console.ReadLine();
        if (comments == "") { comments = null; }
        if (product == "") { product = null; }
        string workDuring = Console.ReadLine();
        DateTime? CreateTime = DateTime.Now;
        int? WorkDuring = (workDuring == "") ? null : int.Parse(workDuring);
        BO.EngineerExperience experience = (BO.EngineerExperience)difficulty;

        BO.Task t = new BO.Task()
        {
            Id = 0,
            Name = name,
            EngineerId = null,
            difficulty = experience,
            StatusTask = null,
            Dependencies = null,
            TaskDescription = taskDescription,
            Product = product,
            Comments = comments,
            CreateTime = CreateTime,
            BeginWorkDateP = null,
            BeginWorkDate = null, 
            WorkDuring = WorkDuring,
            DeadLine = null,
            EndWorkTime = null,
            Engineer = null
        };
        return t;
    }

    //make an engineer object
    private static BO.Engineer makeEngineer()
    {
        Console.WriteLine("enter id, name,Email,level between 0-4 and cost");
        int id = int.Parse(Console.ReadLine());
        string name = Console.ReadLine();
        string Email = Console.ReadLine();
        int level = int.Parse(Console.ReadLine());
        BO.EngineerExperience experience = (BO.EngineerExperience)level;
        double cost = double.Parse(Console.ReadLine());
        BO.Engineer e = new BO.Engineer()
        {
            Id = id,
            Name= name,
            EMail = Email, 
            Level = experience,
            Cost = cost,
            Task = null

        };
        return e;
    }

    //update a task. id values are null: dont update th value.
    private static BO.Task newUpdatedTask(BO.Task taskU)
    {
        if (s_bl.GetStatusProject() == StatusProject.Execution)
        {
            Console.WriteLine("enter name,engineer id,difficulty  between 0-4 ,Product,Comments");
            int id = taskU.Id;
            string name = Console.ReadLine();
            if (name == "")
            {
                name = taskU.Name;
            }
            int engId=int.Parse(Console.ReadLine());
            string difficult = Console.ReadLine();
            BO.EngineerExperience difficulty;
            if (difficult == "") { difficulty = taskU.difficulty; }
            else { difficulty = (BO.EngineerExperience)int.Parse(Console.ReadLine()); }
            string taskDescription = Console.ReadLine();
            if (taskDescription == "") { taskDescription = taskU.TaskDescription; }
            string product = Console.ReadLine();
            if (product == "") { product = taskU.Product; }
            string comments = Console.ReadLine();
            if (comments == "") { comments = taskU.Comments; }           
            BO.Task t = new BO.Task()
            {
                Id = id,
                Name = name,
                EngineerId = engId,
                difficulty = difficulty,
                StatusTask = taskU.StatusTask,
                Dependencies = taskU.Dependencies,
                TaskDescription = taskDescription,
                Product = product,
                Comments = comments,
                CreateTime = taskU.CreateTime,
                BeginWorkDateP = taskU.BeginWorkDateP,
                BeginWorkDate = taskU.BeginWorkDate,
                WorkDuring = taskU.WorkDuring,
                DeadLine = taskU.DeadLine,
                EndWorkTime = taskU.EndWorkTime,
                Engineer = taskU.Engineer
            };
            return t;
        }
        else
        {
            Console.WriteLine("enter name,difficulty  between 0-4 ,Product,Comments ,WorkDuring");
            int id = taskU.Id;
            string name = Console.ReadLine();
            if (name == "")
            {
                name = taskU.Name;
            }

            string difficult = Console.ReadLine();
            BO.EngineerExperience difficulty;
            if (difficult == "") { difficulty = taskU.difficulty; }
            else { difficulty = (BO.EngineerExperience)int.Parse(Console.ReadLine()); }
            string taskDescription = Console.ReadLine();
            if (taskDescription == "") { taskDescription = taskU.TaskDescription; }
            string product = Console.ReadLine();
            if (product == "") { product = taskU.Product; }
            string comments = Console.ReadLine();
            if (comments == "") { comments = taskU.Comments; }
            string workDuring = Console.ReadLine();
            DateTime? CreateTime = taskU.CreateTime;
            int? WorkDuring = (workDuring == "") ? taskU.WorkDuring : int.Parse(workDuring);
            BO.Task t = new BO.Task()
            {
                Id = id,
                Name = name,
                EngineerId = null,
                difficulty = difficulty,
                StatusTask = null,
                Dependencies = null,
                TaskDescription = taskDescription,
                Product = product,
                Comments = comments,
                CreateTime = CreateTime,
                BeginWorkDateP = null,
                BeginWorkDate = null,
                WorkDuring = WorkDuring,
                DeadLine = null,
                EndWorkTime = null,
                Engineer = null
            };
            return t;

        }
    }

    //update a ENGINEER. id values are null: dont update th value.
    private static BO.Engineer newUpdatedEngineer(BO.Engineer engineerU)
    {
        if (s_bl.GetStatusProject() == StatusProject.Execution)
        {
            Console.WriteLine("name,Email,level between 0-4 and cost,task");
            string name = Console.ReadLine();
            if (name == "") { name = engineerU.Name; }
            string Email = Console.ReadLine();
            if (Email == "") { Email = engineerU.EMail; }
            string level = Console.ReadLine();
            BO.EngineerExperience experience;
            if (level == "") { experience = engineerU.Level; }
            else { experience = (BO.EngineerExperience)(int.Parse(level)); }
            double costU;
            string cost = Console.ReadLine();
            if (cost == "") { costU = engineerU.Cost; }
            else { costU = double.Parse(cost); }
            BO.Engineer e = new BO.Engineer()
            {
                Id = engineerU.Id,
                Name = name,
                EMail = Email,
                Level = experience,
                Cost = costU,
                Task = null

            };
            return e;

        }
        else {
            Console.WriteLine("name,Email,level between 0-4 and cost");
            string name = Console.ReadLine();
            if (name == "") { name = engineerU.Name; }
            string Email = Console.ReadLine();
            if (Email == "") { Email = engineerU.EMail; }
            string level = Console.ReadLine();
            BO.EngineerExperience experience;
            if (level == "") { experience = engineerU.Level; }
            else { experience = (BO.EngineerExperience)(int.Parse(level)); }
            double costU;
            string cost = Console.ReadLine();
            if (cost == "") { costU = engineerU.Cost; }
            else { costU = double.Parse(cost); }
            BO.Engineer e = new BO.Engineer()
            {
                Id = engineerU.Id,
                Name = name,
                EMail = Email,
                Level = experience,
                Cost = costU,
                Task = null

            };
            return e;

        }
    }

    //add dependecies to a task the user created
    private static void addDependencies(int taskId)
    {
        //check if it is a manager who want create a dependency and throw exception if it is
        if (s_bl.GetStatusProject() == StatusProject.Execution)
        {
            throw new BO.BlCannotAddDependencyException("nanager cant add a dependency ");
        }
        Console.WriteLine("Do you want to add dependencies to the task? y is true, n is no");
        string answer = Console.ReadLine();
        switch (answer)
        {
            case "y":
                Console.WriteLine("choose id of task your the new task depends on it:");
                string stringId = Console.ReadLine();
                //check id is valid number
                if(!int.TryParse(stringId, out int id)){
                    throw new BlInvalidInputException("id must be a number");
                }
                try
                {
                    s_bl.Task.addDependency(taskId, id);
                }
                catch (BlDoesNotExistException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "n":
                break;
            default:
                throw new BlInvalidInputException("answer must be y or n");
        }
    }
}

