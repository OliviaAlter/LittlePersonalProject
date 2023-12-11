using System.Net;

namespace Common.Exception.HttpException.CustomHttpException;

public abstract class HttpException(HttpStatusCode statusCode, string message, System.Exception innerException)
    : System.Exception(message, innerException)
{
    private HttpStatusCode StatusCode { get; } = statusCode;

    public override string ToString()
    {
        return $"HTTP Error {StatusCode}: {Message}\n{StackTrace}";
    }
}