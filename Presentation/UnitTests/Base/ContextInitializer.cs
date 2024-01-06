using Domain.Base;

namespace UnitTests.Base;

public class ContextInitializer
{
    public CurrentUser CurrentUser { get; }

    public ContextInitializer()
    {
        CurrentUser = new()
        {
            Id = 1,
            FirstName = "Test FirstName",
            LastName = "Test LastName",
            Email = "Test Email",
            IsVerified = true,
            Mobile = "Test Mobile",
            Status = Domain.Enums.UserStatus.Normal
        };
    }
}