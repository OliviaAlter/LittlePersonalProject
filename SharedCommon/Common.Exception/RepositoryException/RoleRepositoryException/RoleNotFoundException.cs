namespace Common.Exception.RepositoryException.RoleRepositoryException;

public class RoleNotFoundException : System.Exception
{
    public RoleNotFoundException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public RoleNotFoundException(string message)
        : base(message)
    {
    }
}