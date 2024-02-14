namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
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
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlInvalidDataException : Exception
{
    public BlInvalidDataException(string? message) : base(message) { }
    public BlInvalidDataException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlTryParseFailedException : Exception
{
    public BlTryParseFailedException(string? message) : base(message) { }
    public BlTryParseFailedException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlOptionDoesntExistException : Exception
{
    public BlOptionDoesntExistException(string? message) : base(message) { }
    public BlOptionDoesntExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlAssignmentFailedException : Exception
{
    public BlAssignmentFailedException(string? message) : base(message) { }
    public BlAssignmentFailedException(string message, Exception innerException)
                : base(message, innerException) { }
}