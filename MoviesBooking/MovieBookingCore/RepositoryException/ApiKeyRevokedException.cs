namespace MovieBookingCore.RepositoryException;

public class ApiKeyRevokedException : Exception
{
    public ApiKeyRevokedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ApiKeyRevokedException(string message)
        : base(message)
    {
    }
}