using Domain.Exceptions.Base;

namespace Domain.Exceptions.UserExceptions;

public class UserNotFoundException : ExceptionBase
{
    public UserNotFoundException()
        : base("UserNotFound", HttpStatusCode.NotFound)
    {
    }
}
