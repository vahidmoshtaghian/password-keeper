using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class DuplicateException : ExceptionBase
{
    public DuplicateException(string entityName)
        : base($"You already entered this {entityName} before", HttpStatusCode.Conflict)
    {

    }
}
