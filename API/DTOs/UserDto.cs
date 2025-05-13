namespace API.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int ExpToNextLevel { get; set; }
    public string Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int BlogCount { get; set; }
}
