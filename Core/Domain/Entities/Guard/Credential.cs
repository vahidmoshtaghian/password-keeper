#nullable disable

using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Guard;

[Table("Credentials", Schema = "Guard")]
public class Credential : EntityBase
{
    [ForeignKey(nameof(Organization))]
    public long OrganizationId { get; set; }

    [ForeignKey(nameof(Password))]
    public long PasswordId { get; set; }

    public Organization Organization { get; set; }
    public Password Password { get; set; }
}
