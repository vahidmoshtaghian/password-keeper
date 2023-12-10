using Domain.Exceptions.Base;

namespace Domain.Exceptions.GuardExceptions;

public class DuplicateEntityException : ExceptionBase
{
    public DuplicateEntityException(string entityName)
        : base($"You already entered {entityName} entity before", HttpStatusCode.Conflict)
    {

    }
}
