using System.Net;
using System.Runtime.Serialization;

namespace CarsAndBids.Core.Exceptions;

[Serializable]
public class HttpException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public HttpException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
    public HttpException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
    public HttpException(string message, HttpStatusCode statusCode, Exception inner) : base(message, inner)
    {
        StatusCode = statusCode;
    }
    protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
