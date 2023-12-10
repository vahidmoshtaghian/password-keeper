#nullable disable

using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Guard;

[Table("Passwords", Schema = "Guard")]
public class Password : EntityBase
{
    public string Value { get; set; }

    public ICollection<UserAccess> UserAccesses { get; set; }
    public ICollection<Credential> Credentials { get; set; }
}
