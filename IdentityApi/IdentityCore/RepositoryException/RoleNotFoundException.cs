namespace IdentityCore.RepositoryException;

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