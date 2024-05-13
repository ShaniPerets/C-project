using BlApi;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;

namespace BO;

public static class Tools
{
    /// <summary>
    /// the function return a string with all the properties and their values of the object. it is 
    /// ans extension method to an object, and it is generic. 
    /// </summary>
    /// <typeparam name="T">type of object the function extend </typeparam>
    /// <param name="obj">object to retuen an string wih all its values of its properties</param>
    /// <returns>a string with all properties and values of an object</returns>
    public static string ToStringProperty<T>(this T obj)
    {
        // Initialize a StringBuilder to construct the resulting string
        var result = new StringBuilder();

        // Get the type of the object
        Type type = typeof(T);

        // Get all properties of the type
        PropertyInfo[] properties = type.GetProperties();

        // Loop through each property
        foreach (var property in properties)
        {
            // Append property name to the result string
            result.Append(property.Name + ": ");

            // Check if the property is readable
            if (property.CanRead)
            {
                // Get the value of the property
                object value = property.GetValue(obj);

                // Check if the value is not null
                if (value != null)
                {
                    if (value is IEnumerable<object> collectionValue)
                    {
                        if (collectionValue.Any()) //check it is has members
                        {
                            result.Append($" \n[{string.Join(", \n ", collectionValue)}]\n");
                        }
                    }

                    // Check if the value is a nested object
                    else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {
                        // Recursively call the method to get properties of nested objects
                        result.Append('\n');
                        result.Append(value.ToString());
                    }
                    else
                    {
                        // Append the string representation of the value to the result
                        result.Append(value.ToString());
                    }
                }
                else
                {
                    // If value is null, indicate that in the result string
                    result.Append("null");
                }
            }
            else
            {
                // If property is not readable, indicate that in the result string
                result.Append("unreadable");
            }

            // Append a new line after each property
            result.Append(Environment.NewLine);
        }

        // Return the constructed string
        return result.ToString();
    }

    /// <summary>
    /// takes DO engineer and convert it to BO engineer
    /// </summary>
    /// <param name="item"></param>
    /// <returns>new BO engineer</returns>
    public static BO.Engineer ConvertToBoEngineer(this DO.Engineer obj, TaskInEngineer task)
    {
        return new BO.Engineer()
        {
            Id = obj.Id,
            Name = obj.Name,
            EMail = obj.EMail,
            Level = (BO.EngineerExperience)obj.Level,
            Cost = obj.Cost,
            Task = task,
        };
    }

    /// <summary>
    /// takes BO engineer and convert it to DO engineer
    /// </summary>
    /// <param name="item"></param>
    /// <returns>new DO engineer</returns>
    public static DO.Engineer ConvertToDoEngineer(this BO.Engineer obj)
    {
        return new DO.Engineer()
        {
            Id = obj.Id,
            Name = obj.Name,
            EMail = obj.EMail,
            Level = (DO.EngineerExperience)obj.Level,
            Cost = obj.Cost,
        };
    }


    /// <summary>
    /// takes DO task and convert it to BO task
    /// </summary>
    /// <param name="item"></param>
    /// <returns>new BO task</returns>
    public static BO.Task ConvertToBoTask(this DO.Task doTask, Status status, List<BO.TaskInList?>? dependencies,  EngineerInTask? engineer)
    {
        
        //return the task by BO level type
        return new BO.Task()
        {
            Id = doTask.Id,
            Name = doTask.Name,
            EngineerId = doTask.EngineerId,
            difficulty = (BO.EngineerExperience)((int)doTask.difficulty),
            TaskDescription = doTask.TaskDescription,
            Product = doTask.Product,
            Comments = doTask.Comments,
            CreateTime = doTask.CreateTime,
            BeginWorkDateP = doTask.BeginWorkDateP,
            BeginWorkDate = doTask.BeginWorkDate,
            WorkDuring = doTask.WorkDuring,
            DeadLine = doTask.DeadLine,
            EndWorkTime = doTask.EndWorkTime,
            StatusTask = status, 
            Dependencies = dependencies, 
            Engineer = engineer
        };
    }

    /// <summary>
    /// takes BO task and convert it to DO task
    /// </summary>
    /// <param name="item"></param>
    /// <returns>new DO task</returns>
    public static DO.Task ConvertToDoTask(this BO.Task item)
    {
        return new DO.Task(item.Id, item.Name, item.EngineerId, (DO.EngineerExperience)((int)item.difficulty), item.TaskDescription, false, item.Product, item.Comments, item.CreateTime, item.BeginWorkDateP, item.BeginWorkDate, item.WorkDuring, item.DeadLine, item.EndWorkTime);
    }
}