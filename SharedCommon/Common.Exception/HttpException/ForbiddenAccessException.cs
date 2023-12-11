namespace Common.Exception.HttpException;

public class ForbiddenAccessException : System.Exception
{
    public ForbiddenAccessException()
    {
    }

    public ForbiddenAccessException(string message) : base(message)
    {
    }

    public ForbiddenAccessException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}