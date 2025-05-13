using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Surname { get; set; } = null!;
    [Required]
    public int Age { get; set; }
    [Required]
    public string Email { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int ExpToNextLevel { get; set; } = 100;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int BlogCount { get; set; } = 0;
    public ICollection<Blog> Blogs { get; set; } = new List<Blog>(); //All blogs belonging to a user are accessible via user.Blogs. (User - Blog one to many relationship)
    public ICollection<BlogLike> LikedBlogs { get; set; } = new List<BlogLike>();
    public ICollection<BlogSave> SavedBlogs { get; set; } = new List<BlogSave>();

}

public enum UserRole
{
    User, Admin
}