using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class Blog
{

    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;
    public string? Subtitle { get; set; }

    [Required]
    public string Description { get; set; } = null!;
    public List<string>? ImageUrls { get; set; } = new();
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //!user relationship
    public int UserId { get; set; } //Foreign key
    public User User { get; set; } = null!; //Navigation property
    public int ViewCount { get; set; } = 0;

    public ICollection<BlogLike> Likes { get; set; } = new List<BlogLike>();

    public int LikeCount { get; set; } = 0;

    public ICollection<BlogSave> Saves { get; set; } = new List<BlogSave>();
    public int SaveCount { get; set; } = 0;

}