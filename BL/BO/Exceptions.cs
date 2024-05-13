

namespace BO;

/// <summary>
/// not valid input exception
/// </summary>
[Serializable]
public class BlInvalidInputException : Exception
{
    public BlInvalidInputException(string? message) : base(message) { }
    public BlInvalidInputException(string? message, Exception innerException) : base(message, innerException) { }
}
/// <summary>
/// already exsist input exception-for example when add task that already exsist
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// not exsist input exception-for example when want read task that not exsist
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// cant delete the input 
/// </summary>
[Serializable]
public class BlCannotDeleteItemException : Exception
{
    public BlCannotDeleteItemException(string? message) : base(message) { }
    public BlCannotDeleteItemException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// not a valid number exeption, for number not in range in switches of choose entities
/// </summary>
[Serializable]
public class BlNotValidNumberException : Exception
{
    public BlNotValidNumberException(string? message) : base(message) { }
    public BlNotValidNumberException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// cannot find the engineer handle the task
/// </summary>
[Serializable]
public class BlCannotFindEngineerOfTaskException : Exception
{
    public BlCannotFindEngineerOfTaskException(string? message) : base(message) { }
    public BlCannotFindEngineerOfTaskException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// cannot update a task
/// </summary>
[Serializable]
public class BlCannotUpdateTaskException : Exception
{
    public BlCannotUpdateTaskException(string? message) : base(message) { }
    public BlCannotUpdateTaskException(string? message, Exception inner) : base(message, inner) { }


}
/// <summary>
/// cannot create a task
/// </summary>
[Serializable]
public class BlCannotCreateTaskException : Exception
{
    public BlCannotCreateTaskException(string? message) : base(message) { }
    public BlCannotCreateTaskException(string? message, Exception innerException) : base(message, innerException) { }
}
/// <summary>
/// cannot add a dependency
/// </summary>
[Serializable]
public class BlCannotAddDependencyException : Exception
{
    public BlCannotAddDependencyException(string? message) : base(message) { }
    public BlCannotAddDependencyException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// cannot update a task
/// </summary>
[Serializable]
public class BlnotValidDateException : Exception
{
    public BlnotValidDateException(string? message) : base(message) { }
    public BlnotValidDateException(string? message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class BlCreateException : Exception
{
    public BlCreateException(string? message) : base(message) { }
    public BlCreateException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// an unknown error while deleting an entity from the dal
/// </summary>
[Serializable]
public class BlDeleteException : Exception
{
    public BlDeleteException(string? message) : base(message) { }
    public BlDeleteException(string? message, Exception innerException) : base(message, innerException) { }

}

/// <summary>
/// an unknown error while updating an entity from the dal
/// </summary>

[Serializable]
public class BlUpdateException : Exception
{
    public BlUpdateException(string? message) : base(message) { }
    public BlUpdateException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// according to the premissions cant do changes
/// </summary>
[Serializable]
public class BlUnAuthrizedAccessExpetion :Exception
{
    public BlUnAuthrizedAccessExpetion(string? message) : base(message) { }
    public BlUnAuthrizedAccessExpetion(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// there is a dependency loop
/// </summary>
[Serializable]
public class BlLoopDependencyExpetion : Exception
{
    public BlLoopDependencyExpetion(string? message) : base(message) { }
    public BlLoopDependencyExpetion(string? message, Exception innerException) : base(message, innerException) { }
}
