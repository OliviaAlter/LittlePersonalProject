namespace IdentityCore.RepositoryException;

// ApiKey
public class ApiKeyGenerationException : Exception
{
    public ApiKeyGenerationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public class ApiKeyValidationException : Exception
{
    public ApiKeyValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

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

// Roles
public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public RoleNotFoundException(string message)
        : base(message)
    {
    }
}

public class RoleCreationException : Exception
{
    public RoleCreationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public RoleCreationException(string message)
        : base(message)
    {
    }
}

// Users