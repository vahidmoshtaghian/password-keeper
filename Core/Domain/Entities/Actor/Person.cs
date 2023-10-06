using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Actor;

public class Person : EntityBase
{
    [Required, MinLength(2), MaxLength(100)]
    public string FirstName { get; set; }

    [Required, MinLength(2), MaxLength(100)]
    public string LastName { get; set; }

    [Required, MinLength(11), MaxLength(11)]
    public string Mobile { get; set; }
}
