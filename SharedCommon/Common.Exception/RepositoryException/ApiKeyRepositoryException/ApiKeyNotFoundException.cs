namespace Common.Exception.RepositoryException.ApiKeyRepositoryException;

public class ApiKeyNotFoundException : System.Exception
{
    public ApiKeyNotFoundException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public ApiKeyNotFoundException(string message)
        : base(message)
    {
    }
}