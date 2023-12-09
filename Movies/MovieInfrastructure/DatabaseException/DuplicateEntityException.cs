namespace MovieInfrastructure.DatabaseException;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DuplicateEntityException(string message)
        : base(message)
    {
    }
}