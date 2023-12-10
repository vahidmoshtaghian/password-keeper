#nullable disable

using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Actor;

[Table("People", Schema = "Actor")]
public class Person : EntityBase
{
    [Required, MinLength(2), MaxLength(100)]
    public string FirstName { get; set; }

    [Required, MinLength(2), MaxLength(100)]
    public string LastName { get; set; }

    [Required, MinLength(11), MaxLength(11), Phone]
    public string Phone { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
