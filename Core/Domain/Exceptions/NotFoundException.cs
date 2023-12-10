using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class NotFoundException : ExceptionBase
{
    public NotFoundException(string entityName)
        : base($"{entityName} not found!", HttpStatusCode.NotFound)
    {

    }
}
