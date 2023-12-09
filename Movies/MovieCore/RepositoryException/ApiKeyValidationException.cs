namespace MovieCore.RepositoryException;

public class ApiKeyValidationException : Exception
{
    public ApiKeyValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}