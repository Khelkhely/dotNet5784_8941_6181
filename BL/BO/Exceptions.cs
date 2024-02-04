namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlInvalidInputException : Exception
{
    public BlInvalidInputException(string? message) : base(message) { }
    public BlInvalidInputException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlCanNotDeleteException : Exception
{
    public BlCanNotDeleteException(string? message) : base(message) { }
    public BlCanNotDeleteException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlTaskDateException : Exception
{
    public BlTaskDateException(string? message) : base(message) { }
    public BlTaskDateException(string message, Exception innerException)
                : base(message, innerException) { }
}