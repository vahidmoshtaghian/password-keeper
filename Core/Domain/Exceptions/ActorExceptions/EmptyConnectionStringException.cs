using Domain.Exceptions.Base;

namespace Domain.Exceptions.UserExceptions;

public class EmptyConnectionStringException : ExceptionBase
{
    public EmptyConnectionStringException() : base("Connection string is empty")
    {

    }
}
