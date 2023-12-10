namespace MovieBookingCore.RepositoryException;

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