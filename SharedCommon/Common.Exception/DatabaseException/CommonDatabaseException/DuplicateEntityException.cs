namespace Common.Exception.DatabaseException.CommonDatabaseException;

public class DuplicateEntityException : System.Exception
{
    public DuplicateEntityException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public DuplicateEntityException(string message)
        : base(message)
    {
    }
}