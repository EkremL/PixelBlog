namespace API.Entities
{

    public class BlogLike
    {

        public int UserId { get; set; }
        public int BlogId { get; set; }  // Since we have two foreign keys (UserId and BlogId) and no single Id property,
                                         // we define a composite primary key using both fields in DataContext.cs
        public User User { get; set; } = null!;
        public Blog Blog { get; set; } = null!;

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }

}