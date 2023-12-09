namespace MovieInfrastructure.DatabaseException;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }
}