#nullable disable

using Domain.Base;
using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Guard;

[Table("Organizations", Schema = "Guard")]
public class Organization : EntityBase, IUpdatable, IDeletable
{
    [MinLength(2), MaxLength(100)]
    public string Title { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<Membership> Memberships { get; set; }
    public ICollection<Credential> Credentials { get; set; }
}
