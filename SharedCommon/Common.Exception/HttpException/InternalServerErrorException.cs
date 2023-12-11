namespace Common.Exception.HttpException;

public class InternalServerErrorException : System.Exception
{
    public InternalServerErrorException()
    {
    }

    public InternalServerErrorException(string message) : base(message)
    {
    }

    public InternalServerErrorException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}