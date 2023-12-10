#nullable disable

using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Guard;

[Table("UserAccesses", Schema = "Guard")]
public class UserAccess : EntityBase
{
    public bool IsCreator { get; set; }

    [ForeignKey(nameof(Membership))]
    public long MembershipId { get; set; }
    public Membership Membership { get; set; }

    [ForeignKey(nameof(Password))]
    public long PasswordId { get; set; }
    public Password Password { get; set; }
}
