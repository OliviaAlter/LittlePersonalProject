namespace IdentityInfrastructure.DatabaseException;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DuplicateEntityException(string message)
        : base(message)
    {
    }
}

public class DatabaseOperationException : Exception
{
    public DatabaseOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DatabaseOperationException(string message)
        : base(message)
    {
    }
}

public class RepositoryException : Exception
{
    public RepositoryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public RepositoryException(string message)
        : base(message)
    {
    }
}

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }
}

public class PasswordHashingException : Exception
{
    public PasswordHashingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public PasswordHashingException(string message)
        : base(message)
    {
    }
}