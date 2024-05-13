
namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }

}

[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }

}

[Serializable]
public class DalDeletionImpossible : Exception
{
    public DalDeletionImpossible(string? message) : base(message) { }

}

/// <summary>
/// not a valid number exeption, for number not in range in switches of choose entities
/// </summary>
[Serializable]
public class DalNotValidNumber : Exception
{
    public DalNotValidNumber(string? message) : base(message) { }

}

//exception for the XMLTools
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}

//exception for task cannot be deleted
[Serializable]
public class DalDeletionImpossibleException : Exception
{
    public DalDeletionImpossibleException(string? message) : base(message) { }
}

//exception for error in config - use it in Factory class.
[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string? message) : base(message) { }
}

[Serializable]
public class DalCannotUpdateException : Exception
{
    public DalCannotUpdateException(string? message) : base(message) { }
}