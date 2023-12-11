namespace Common.Exception.HttpException;

public class UnsupportedMediaTypeException : System.Exception
{
    public UnsupportedMediaTypeException()
    {
    }

    public UnsupportedMediaTypeException(string message) : base(message)
    {
    }

    public UnsupportedMediaTypeException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}