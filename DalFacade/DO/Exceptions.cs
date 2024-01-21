using System.Runtime.Serialization;

namespace DO;

public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

public class DalNoSuchEnumExistsException : Exception
{
    public DalNoSuchEnumExistsException(string? message) : base(message) { }
}

[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message)  { }

    public DalXMLFileLoadCreateException(string? message, Exception? innerException) : base(message, innerException) { }
}