using Domain.Exceptions.Base;

namespace Domain.Exceptions.UserExceptions;

public class PasswordIsWrongException : ExceptionBase
{
    public PasswordIsWrongException()
        : base("Password is wrong", HttpStatusCode.Forbidden)
    {
    }
}
