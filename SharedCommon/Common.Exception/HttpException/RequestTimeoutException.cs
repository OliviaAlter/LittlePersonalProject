namespace Common.Exception.HttpException;

public class RequestTimeoutException : System.Exception
{
    public RequestTimeoutException()
    {
    }

    public RequestTimeoutException(string message) : base(message)
    {
    }

    public RequestTimeoutException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}