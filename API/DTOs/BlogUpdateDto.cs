namespace API.DTOs
{
    public class BlogUpdateDto
    {
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string Description { get; set; } = null!;
        public List<string>? ImageUrls { get; set; }
    }
}
