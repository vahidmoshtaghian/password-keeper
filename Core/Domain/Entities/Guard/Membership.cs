#nullable disable

using Domain.Entities.Actor;
using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Guard;

[Table("Memberships", Schema = "Guard")]
public class Membership : EntityBase
{
    public bool IsOwner { get; set; }

    [ForeignKey(nameof(User))]
    public long UserId { get; set; }

    [ForeignKey(nameof(Organization))]
    public long OrganizationId { get; set; }

    public User User { get; set; }
    public Organization Organization { get; set; }

    public ICollection<UserAccess> UserAccesses { get; set; }
}
