using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class BlogCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string Description { get; set; } = null!;
        public List<string>? ImageUrls { get; set; }

    }
}
