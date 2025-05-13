using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BlogReadLog> BlogReadLogs { get; set; }
    public DbSet<BlogLike> BlogLikes { get; set; }
    public DbSet<BlogSave> BlogSaves { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //! Call the base configuration to ensure default behavior is preserved
        base.OnModelCreating(modelBuilder);

        //! Configure BlogLike to use a composite primary key made up of UserId and BlogId  ex:“User 5 → has liked Blog 2”
        //! This ensures that each user can like a blog only once  
        modelBuilder.Entity<BlogLike>().HasKey(bl => new { bl.UserId, bl.BlogId }); //Composite key for like
        modelBuilder.Entity<BlogSave>().HasKey(bs => new { bs.UserId, bs.BlogId }); //for save

    }
}