namespace Common.Exception.HttpException;

public class NotImplementedErrorException : System.Exception
{
    public NotImplementedErrorException()
    {
    }

    public NotImplementedErrorException(string message) : base(message)
    {
    }

    public NotImplementedErrorException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}