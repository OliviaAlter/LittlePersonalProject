namespace Common.Exception.DatabaseException.CommonDatabaseException;

public class EntityNotFoundException : System.Exception
{
    public EntityNotFoundException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }
}