using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Base;

public class EntityBase
{
    public EntityBase()
    {
        CreatedAt = DateTime.Now;
    }

    [Key]
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
