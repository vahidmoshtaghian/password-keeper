using Domain.Exceptions.Base;

namespace Domain.Exceptions.GuardExceptions;

public class AccessDeniedException : ExceptionBase
{
    public AccessDeniedException() : base("You don't have access to do that", HttpStatusCode.Forbidden)
    {

    }
}
