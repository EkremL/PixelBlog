namespace API.DTOs
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string Description { get; set; } = null!;
        public List<string>? ImageUrls { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ViewCount { get; set; } = 0;
        public int UserId { get; set; }
        public string AuthorUsername { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public string AuthorSurname { get; set; } = null!;
        public string AuthorAvatar { get; set; } = null!;
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public int SaveCount { get; set; }
        public bool IsSavedByCurrentUser { get; set; }


    }
}
