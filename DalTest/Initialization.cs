namespace DalTest;
using DalApi;
using DO;
using System;
using System.Data.Common;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;


/// <summary>
/// initialize all the database with random tasks, engineers, and dependencies.
/// </summary>
/// 
public static class Initialization
{
    const int NumEngineers = 6; //number of engineers in the program

    //an inteface type wrapping actions of engineers, tasks, dependencies (IDependency, IEngineer, ITask)
    private static IDal? _s_dal;

    private static readonly Random s_rand = new();

    //create a task initialize all fields with random values.
    private static void createTasks()
    {
        string[] taskNames = { "algorithm problem", "stabilizing", "accounting", "kitchenChanges","shopping","task1" , "task2" , "task3" , "task4" , "task5" , "task6" , "task7" , "task8" , "task9","task10","task11","task12","task13","task14","task15" };
        string[] tasksDescription = { "solve an algorithm problem of stabilizing", "find a way to stabilize the park", "calculation of expenses and incomes", "change the kitchen structure","go to the market","dec1", "dec2", "dec3", "dec4", "dec5", "dec6", "dec7", "dec8", "dec9", "dec10", "dec11", "dec12", "dec13", "dec14", "dec15" };
        string[] products = { "a suggest of stabilizing the column", "a way to stabilize the park", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money", "save money" };
        string?[] commentsArr = { "it is a very hard task!", "very easy to complete", "the task takes only one day to fulfill!", "please notice to the notes of the manager", "notice the style is vey important", null, null,null, null, null, null, null, null, null, null, null, null, null, null, null };

        int index = 0;

        //for any task initialize it with random numbers. add it to the database.
        foreach (var _name in taskNames)
        {
            int? EngineerId = null;
            EngineerExperience difficulty = (EngineerExperience)s_rand.Next(0, 5);
            string TaskDescription = tasksDescription[index];
            bool MileStone = false;
            string? Product = products[index];
            string? Comments = commentsArr[s_rand.Next(0, commentsArr.Length)];
            int? WorkDuring = s_rand.Next(0, 200);
            Task newTask = new(0, _name, EngineerId, difficulty, TaskDescription, MileStone, Product, Comments, DateTime.Now, null, null, WorkDuring,null, null);
            _s_dal!.Task.Create(newTask);

            index++;
        }

    }

    //initialize dependencies with random tasks chosen. add it to database.
    private static void createDependencys()
    {
        int DependentTask;  //the task that depends on another task
        int DependsOnTask;  //the task that another task depends on.
        int makeRandom = s_rand.Next(0, 400);
        List<Task?> tasks = (List<DO.Task?>)(_s_dal!.Task.ReadAll().ToList());

        //make a randomly dependcys between tasks.
        foreach (Task? task in tasks)
        {
            if (task != null)
            {
                makeRandom = s_rand.Next(0, 400);
                if (makeRandom % 3 == 0)
                {
                    DependentTask =  task.Id;
                    foreach (Task? task2 in tasks)
                    {
                        if(task2.Id <= task.Id)
                        {
                            continue;
                        }
                        if (task2 != null && task2 != task)
                        {
                            //task2 bein after task1 end
                                makeRandom = s_rand.Next(0, 400);
                                //randomly choose if a tas will be depend on task1 or not
                                if (makeRandom % 4 == 0)
                                {
                                    DependsOnTask = task2.Id;
                                    Dependency newDepend = new(0, DependentTask, DependsOnTask);
                                    bool toCreate = true;
                                    List<Dependency?> depends = (List<DO.Dependency?>)(_s_dal!.Dependency.ReadAll().ToList());
                                    //check the dependency doesnt exist already
                                    foreach (Dependency depend in depends)
                                    {
                                        if (depend.DependentTask == DependentTask && depend.DependsOnTask == DependsOnTask)
                                        {
                                            toCreate = false;
                                        }
                                        //not a loop dependency between 2 tasks
                                    if (depend.DependentTask == DependsOnTask && depend.DependsOnTask == DependentTask)
                                    {
                                        toCreate = false;
                                    }
                                }
                                    //create only if dependency doesnt already exist
                                    if (toCreate) { _s_dal!.Dependency.Create(newDepend); };
                                }
                            }

                        }
                    
                }

            }
        }
        
        //make sure there are at least 2 dependent that dependson list of them is the same:
        List<Dependency> listDepends = (List<Dependency>)(_s_dal!.Dependency.ReadAll().ToList());
        int maxDependency = listDepends.Count;

        //if the list is empty: try fill it again with the function
        while (((List<Dependency>)(_s_dal!.Dependency.ReadAll().ToList())).Count < 40)//suppose to be 40 but there are not still enough tasks
        {
            createDependencys();
        }

        
    }

    //initialize engineers in the database.
    private static void createEngineers()
    {
        //array with names and emails of engineers
        string[] names = { "Tomer", "Batia", "Itamar", "Refael", "Bar", "Judi" };
        string[] emails = { "tomer@gmail.com", "BatiR@gmail.co.il", "ItAmar@gmail.com", "Rfa@gmail.com", "barERT@gmail.com", "judasjud@gmail.co.il" };

        //create a random id with 8 digits
        int id = s_rand.Next(10000000, 100000000);
        
        string name = "";
        string email = "";

        for (int i = 0; i < names.Length; i++)
        {
            //make a random id that does not exist
            while (_s_dal!.Engineer.Read(id) is not null)
            {
                id = s_rand.Next(10000000, 100000000);
            }
            //set the appropriate name and email of engineer:
            name = names[i];
            email = emails[i];
            //level of experience
            EngineerExperience level = (EngineerExperience)s_rand.Next(0, 5); ;
            double cost = s_rand.Next(0, 30000);
            Engineer newEngineer = new(id, name, email, level, cost);
            _s_dal!.Engineer.Create(newEngineer);

        }

    }
    //intialzatw the lists data base
    public static void Do() 
    {

        //_s_dal wrap all interfaces: ITask, IDependency, IEngineer.
        //factory will implement the _s_dal to the way we want: xml/list etc according to config.
        _s_dal = DalApi.Factory.Get;

        //remove all items before intialate
        _s_dal.Dependency.RemoveAll();
        _s_dal.Task.RemoveAll();
        _s_dal.Engineer.RemoveAll();

        //initialate after clearing
        createEngineers();
        createTasks();
        createDependencys();
    }

    //reset the database
    public static void Reset()
    {
        _s_dal = DalApi.Factory.Get;

        //remove all items from db
        _s_dal.Dependency.RemoveAll();
        _s_dal.Task.RemoveAll();
        _s_dal.Engineer.RemoveAll();
    }
}