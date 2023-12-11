namespace Common.Exception.RepositoryException.CommonRepositoryException;

public class RepositoryException : System.Exception
{
    public RepositoryException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public RepositoryException(string message)
        : base(message)
    {
    }
}