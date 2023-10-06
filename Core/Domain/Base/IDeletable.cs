namespace Domain.Base;

public interface IDeletable
{
    DateTime? DeletedAt { get; protected set; }

    void Delete()
    {
        DeletedAt = DateTime.Now;
    }
    void Restore()
    {
        DeletedAt = null;
    }
}
