namespace Infrastructure.DatabaseException;

public class DatabaseOperationException : Exception
{
    public DatabaseOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DatabaseOperationException(string message)
        : base(message)
    {
    }
}