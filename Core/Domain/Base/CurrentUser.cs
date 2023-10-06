using Domain.Enums;

namespace Domain.Base;

public class CurrentUser
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public UserStatus Status { get; set; }
    public bool IsVerified { get; set; }
}
