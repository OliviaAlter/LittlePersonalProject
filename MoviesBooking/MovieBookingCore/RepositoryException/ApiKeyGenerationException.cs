namespace Core.RepositoryException;

// ApiKey
public class ApiKeyGenerationException : Exception
{
    public ApiKeyGenerationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

// Roles

// Users