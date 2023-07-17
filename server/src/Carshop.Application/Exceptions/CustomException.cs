using System.Net;

namespace Carshop.Application.Exceptions;

public class CustomException : Exception
{
    public HttpStatusCode Status { get; private set; }

    public CustomException(string message, HttpStatusCode status) : base(message)
    {
        Status = status;
    }
}