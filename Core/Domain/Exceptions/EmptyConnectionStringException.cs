using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class EmptyConnectionStringException : ExceptionBase
{
	public EmptyConnectionStringException() : base("Connection string is empty")
	{

	}
}
