//!This table will keep track of who read which blog and when and it will be related with BlogService.cs

namespace API.Entities;

public class BlogReadLog
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int BlogId { get; set; }
    public Blog? Blog { get; set; }

    public DateTime ReadAt { get; set; } = DateTime.UtcNow;
}
