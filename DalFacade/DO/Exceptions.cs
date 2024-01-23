using System.Runtime.Serialization;

namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
    public DalDoesNotExistException(string? message, Exception? innerException) : base(message, innerException) { }

}

[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
    public DalAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException) { }

}

[Serializable]
public class DalNoSuchEnumExistsException : Exception
{
    public DalNoSuchEnumExistsException(string? message) : base(message) { }
    public DalNoSuchEnumExistsException(string? message, Exception? innerException) : base(message, innerException) { }

}

[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message)  { }

    public DalXMLFileLoadCreateException(string? message, Exception? innerException) : base(message, innerException) { }
}

[Serializable]
public class DalOptionDoesntExist : Exception
{
    public DalOptionDoesntExist(string? message) : base(message) { }

    public DalOptionDoesntExist(string? message, Exception? innerException) : base(message, innerException) { }
}

[Serializable]
public class DalTryParseFailed : Exception
{
    public DalTryParseFailed(string? message) : base(message) { }

    public DalTryParseFailed(string? message, Exception? innerException) : base(message, innerException) { }
}