#nullable disable

using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Actor;

[Table("Friends", Schema = "Actor")]
public class Friend : EntityBase
{
    [Required, MinLength(2), MaxLength(100)]
    public string FirstName { get; set; }

    [Required, MinLength(2), MaxLength(100)]
    public string LastName { get; set; }

    [ForeignKey(nameof(User))]
    public long UserId { get; set; }

    public User User { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
