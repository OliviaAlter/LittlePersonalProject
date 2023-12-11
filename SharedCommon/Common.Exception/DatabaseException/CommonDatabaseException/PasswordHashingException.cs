namespace Common.Exception.DatabaseException.CommonDatabaseException;

public class PasswordHashingException : System.Exception
{
    public PasswordHashingException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public PasswordHashingException(string message)
        : base(message)
    {
    }
}