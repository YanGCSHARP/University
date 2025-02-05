namespace WebProj2.Models;

public class BaseDbItem
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public BaseDbItem()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}