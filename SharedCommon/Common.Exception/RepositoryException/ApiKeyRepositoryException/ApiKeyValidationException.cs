namespace Common.Exception.RepositoryException.ApiKeyRepositoryException;

public class ApiKeyValidationException : System.Exception
{
    public ApiKeyValidationException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}