namespace Common.Exception.DatabaseException.CommonDatabaseException;

public class DatabaseOperationException : System.Exception
{
    public DatabaseOperationException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public DatabaseOperationException(string message)
        : base(message)
    {
    }
}