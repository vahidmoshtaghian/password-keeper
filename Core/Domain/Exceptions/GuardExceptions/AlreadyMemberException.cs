using Domain.Exceptions.Base;

namespace Domain.Exceptions.GuardExceptions;

public class AlreadyMemberException : ExceptionBase
{
    public AlreadyMemberException()
        : base("This member is already in the organization.", HttpStatusCode.Conflict) { }
}
