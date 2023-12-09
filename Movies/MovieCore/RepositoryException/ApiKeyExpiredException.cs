namespace MovieCore.RepositoryException;

public class ApiKeyExpiredException : Exception
{
    public ApiKeyExpiredException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ApiKeyExpiredException(string message)
        : base(message)
    {
    }
}