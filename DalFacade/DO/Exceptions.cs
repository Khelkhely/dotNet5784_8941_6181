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