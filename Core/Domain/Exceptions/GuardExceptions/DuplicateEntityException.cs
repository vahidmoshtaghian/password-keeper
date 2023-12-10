using Domain.Exceptions.Base;

namespace Domain.Exceptions.GuardExceptions;

public class DuplicateEntityException : ExceptionBase
{
    public DuplicateEntityException(string name)
        : base($"You already entered {name} entity before", HttpStatusCode.Conflict)
    {

    }
}
