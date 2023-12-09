namespace Core.RepositoryException;

public class ApiKeyNotFoundException : Exception
{
    public ApiKeyNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ApiKeyNotFoundException(string message)
        : base(message)
    {
    }
}