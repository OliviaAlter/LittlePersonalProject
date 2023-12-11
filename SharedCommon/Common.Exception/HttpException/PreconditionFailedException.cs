namespace Common.Exception.HttpException;

public class PreconditionFailedException : System.Exception
{
    public PreconditionFailedException()
    {
    }

    public PreconditionFailedException(string message) : base(message)
    {
    }

    public PreconditionFailedException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}