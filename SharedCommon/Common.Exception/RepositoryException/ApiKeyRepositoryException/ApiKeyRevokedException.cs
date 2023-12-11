namespace Common.Exception.RepositoryException.ApiKeyRepositoryException;

public class ApiKeyRevokedException : System.Exception
{
    public ApiKeyRevokedException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public ApiKeyRevokedException(string message)
        : base(message)
    {
    }
}