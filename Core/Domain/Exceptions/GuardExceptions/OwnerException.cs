using Domain.Exceptions.Base;

namespace Domain.Exceptions.GuardExceptions;

public class OwnerException : ExceptionBase
{
    public OwnerException()
        : base("You are not owner", HttpStatusCode.Forbidden) { }
}
