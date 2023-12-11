namespace Common.Exception.RepositoryException.ApiKeyRepositoryException;

public class ApiKeyExpiredException : System.Exception
{
    public ApiKeyExpiredException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public ApiKeyExpiredException(string message)
        : base(message)
    {
    }
}