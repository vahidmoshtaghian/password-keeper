using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class CustomException : ExceptionBase
{
    public CustomException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode)
    {

    }
}
