namespace Infrastructure.DatabaseException;

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