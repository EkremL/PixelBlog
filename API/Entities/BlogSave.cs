namespace API.Entities;

public class BlogSave
{
    public int Id { get; set; }

    // Foreign Keys
    public int BlogId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public Blog Blog { get; set; } = null!;
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}
