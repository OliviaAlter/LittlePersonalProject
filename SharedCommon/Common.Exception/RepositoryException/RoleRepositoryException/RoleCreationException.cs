namespace Common.Exception.RepositoryException.RoleRepositoryException;

public class RoleCreationException : System.Exception
{
    public RoleCreationException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public RoleCreationException(string message)
        : base(message)
    {
    }
}