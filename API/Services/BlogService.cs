using API.Data;
using API.DTOs;
using API.Entities;
using API.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

//!This service is handling complex operations related to blogs.
public class BlogService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public BlogService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BlogDto?> ReadBlogByIdAsync(int blogId, int userId)
    {
        //! Fetch the blog by ID, including the related user (author)
        var blog = await _context.Blogs
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == blogId);

        //! If blog doesn't exist, return null
        // if (blog == null)
        //     return null;

        if (blog == null)
            throw new ApiException(404, "Blog not found", $"No blog exists with ID = {blogId}.");

        //! If the user is the blog owner, don't give XP or increase view count
        if (blog.UserId == userId)
            return _mapper.Map<BlogDto>(blog);

        //! Check if the user has already read this blog
        var hasRead = await _context.BlogReadLogs
            .AnyAsync(x => x.UserId == userId && x.BlogId == blogId); //!this will return true or false

        if (!hasRead)
        {
            //! Increase blog view count
            blog.ViewCount++;

            //! Find the current user and give XP (+10 for reading a blog)
            var user = await _context.Users.FindAsync(userId);
            // if (user != null)
            // {
            //     UpdateUserExperience(user, 10);
            // }

            if (user == null)
                throw new ApiException(404, "User not found", $"No user exists with ID = {userId}.");

            UpdateUserExperience(user, 10);

            //! Log that the user read this blog for prevent gain xp from same blog (hasRead doing this job but we ensure that we are saving this data to another table in db.)
            var readLog = new BlogReadLog
            {
                UserId = userId,
                BlogId = blogId,
                ReadAt = DateTime.UtcNow
            };

            _context.BlogReadLogs.Add(readLog);
            await _context.SaveChangesAsync();
        }
        //! Calculate LikeCount and User Has Liked ?
        var dto = _mapper.Map<BlogDto>(blog);
        dto.LikeCount = blog.LikeCount;
        dto.IsLikedByCurrentUser = await _context.BlogLikes.AnyAsync(x => x.BlogId == blog.Id && x.UserId == userId);

        //! Return blog with updated data
        return dto;
    }

    //!Update user exp function (copied from controller)
    private void UpdateUserExperience(User user, int gainedExp)
    {
        user.Experience += gainedExp;

        while (user.Experience >= user.ExpToNextLevel)
        {
            user.Experience -= user.ExpToNextLevel;
            user.Level++;
            user.ExpToNextLevel = user.Level * 100;
        }

        user.UpdatedAt = DateTime.UtcNow;
    }


    //!Create blog logic
    public async Task<BlogDto?> CreateBlogAsync(BlogCreateDto blogCreateDto, int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        // if (user == null)
        //     return null;

        if (user == null)
            throw new ApiException(404, "User not found", $"Cannot create blog: user ID = {userId} not found.");

        var blog = _mapper.Map<Blog>(blogCreateDto);
        blog.UserId = userId;

        // +30 XP
        UpdateUserExperience(user, 30);

        // BlogCount++
        user.BlogCount++;

        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();

        return _mapper.Map<BlogDto>(blog);
    }


    //!Blog Like System Logic
    public async Task<bool> ToggleLikeAsync(int blogId, int userId)
    {
        return await ToggleRelationAsync(
            _context.BlogLikes,
            blogId,
            userId,
            blog => blog.LikeCount,
            (blog, value) => blog.LikeCount = value,
            (uid, bid) => new BlogLike { UserId = uid, BlogId = bid }
        );
    }

    //!Blog Save System Logic
    public async Task<bool> ToggleSaveAsync(int blogId, int userId)
    {
        return await ToggleRelationAsync(
            _context.BlogSaves,
            blogId,
            userId,
            blog => blog.SaveCount,
            (blog, value) => blog.SaveCount = value,
            (uid, bid) => new BlogSave { UserId = uid, BlogId = bid }
        );
    }
    /// <summary>
    /// Toggles a relational action (like/save) between a user and a blog post.
    /// Can be used for BlogLike, BlogSave or any similar entity with a (UserId, BlogId) composite key.
    /// </summary>
    private async Task<bool> ToggleRelationAsync<TEntity>(
    DbSet<TEntity> dbSet,
    int blogId,
    int userId,
    Func<Blog, int> getCount,
    Action<Blog, int> setCount,
    Func<int, int, TEntity> createEntity)
    where TEntity : class
    {
        //! Find the blog using the blogId
        var blog = await _context.Blogs.FindAsync(blogId);
        if (blog == null)
            throw new ApiException(404, "Blog not found", $"No blog exists with ID = {blogId}.");

        //! Try to find an existing relation (e.g., like/save) using composite key (UserId + BlogId)
        var existing = await dbSet
            .FindAsync(userId, blogId);

        //! If it already exists, remove the relation and decrement the counter
        if (existing != null)
        {
            dbSet.Remove(existing);
            setCount(blog, getCount(blog) - 1);
            await _context.SaveChangesAsync();
            return false;
        }
        //! Otherwise, create a new relation (like/save), add it, and increment the counter
        var newRelation = createEntity(userId, blogId);  // Create new BlogLike or BlogSave
        dbSet.Add(newRelation); // Add to context
        setCount(blog, getCount(blog) + 1); // Increase LikeCount or SaveCount


        await _context.SaveChangesAsync();
        return true;
    }

    //! Retrieve all blogs with like and save information (count and user state)
    public async Task<List<BlogDto>> GetAllBlogsAsync(int? userId)
    {
        //! Fetch all blogs including their authors, sorted by newest first
        var blogs = await _context.Blogs
            .Include(b => b.User)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        var blogDtos = new List<BlogDto>();

        //! Extract all blog IDs for further filtering
        var blogIds = blogs.Select(b => b.Id).ToList();

        //! These lists will hold the IDs of blogs liked and saved by the current user
        var likedBlogIds = new List<int>();
        var savedBlogIds = new List<int>();

        //! If the user is authenticated, fetch their liked and saved blogs in a single query
        if (userId != null)
        {
            likedBlogIds = await _context.BlogLikes
                .Where(x => x.UserId == userId && blogIds.Contains(x.BlogId))
                .Select(x => x.BlogId)
                .ToListAsync();

            savedBlogIds = await _context.BlogSaves
                .Where(x => x.UserId == userId && blogIds.Contains(x.BlogId))
                .Select(x => x.BlogId)
                .ToListAsync();
        }

        //! Loop through each blog and map to BlogDto, adding dynamic like/save info
        foreach (var blog in blogs)
        {
            var dto = _mapper.Map<BlogDto>(blog);

            dto.LikeCount = blog.LikeCount;
            dto.IsLikedByCurrentUser = likedBlogIds.Contains(blog.Id);

            dto.SaveCount = blog.SaveCount;
            dto.IsSavedByCurrentUser = savedBlogIds.Contains(blog.Id);

            blogDtos.Add(dto);
        }

        //! Return the list of blog DTOs enriched with like/save data
        return blogDtos;
    }

    //! Get all blogs liked by current user
    public async Task<List<BlogDto>> GetLikedBlogsAsync(int userId)
    {
        //! Find blog IDs liked by the user
        var likedBlogIds = await _context.BlogLikes
            .Where(like => like.UserId == userId)
            .Select(like => like.BlogId)
            .ToListAsync();

        //! Among those liked, check which are also saved by the user
        var savedBlogIds = await _context.BlogSaves
            .Where(save => save.UserId == userId && likedBlogIds.Contains(save.BlogId))
            .Select(save => save.BlogId)
            .ToListAsync();

        //! Fetch liked blogs with author data
        var likedBlogs = await _context.Blogs
            .Where(blog => likedBlogIds.Contains(blog.Id))
            .Include(blog => blog.User)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        //! Prepare DTOs enriched with like/save flags
        var dtoList = new List<BlogDto>();
        foreach (var blog in likedBlogs)
        {
            var dto = _mapper.Map<BlogDto>(blog);
            dto.LikeCount = blog.LikeCount;
            dto.IsLikedByCurrentUser = true; // Already liked
            dto.SaveCount = blog.SaveCount;
            dto.IsSavedByCurrentUser = savedBlogIds.Contains(blog.Id); // Maybe saved too
            dtoList.Add(dto);
        }

        return dtoList;
    }

    //! Get all blogs saved by current user
    public async Task<List<BlogDto>> GetSavedBlogsAsync(int userId)
    {
        //! Get IDs of blogs saved by the user
        var savedBlogIds = await _context.BlogSaves
            .Where(save => save.UserId == userId)
            .Select(save => save.BlogId)
            .ToListAsync();

        //! For saved blogs, check which are also liked by user
        var likedBlogIds = await _context.BlogLikes
            .Where(like => like.UserId == userId && savedBlogIds.Contains(like.BlogId))
            .Select(like => like.BlogId)
            .ToListAsync();

        //! Fetch blog data for those saved blogs
        var savedBlogs = await _context.Blogs
            .Where(blog => savedBlogIds.Contains(blog.Id))
            .Include(blog => blog.User)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        //! Prepare DTOs with enriched info
        var dtoList = new List<BlogDto>();
        foreach (var blog in savedBlogs)
        {
            var dto = _mapper.Map<BlogDto>(blog);
            dto.LikeCount = blog.LikeCount;
            dto.IsLikedByCurrentUser = likedBlogIds.Contains(blog.Id);
            dto.SaveCount = blog.SaveCount;
            dto.IsSavedByCurrentUser = true; // Already saved
            dtoList.Add(dto);
        }

        return dtoList;
    }

    //! Get all blogs that are BOTH liked and saved by the current user
    public async Task<List<BlogDto>> GetLikedAndSavedBlogsAsync(int userId)
    {
        //! Get both liked and saved blog IDs
        var likedBlogIds = await _context.BlogLikes
            .Where(x => x.UserId == userId)
            .Select(x => x.BlogId)
            .ToListAsync();

        var savedBlogIds = await _context.BlogSaves
            .Where(x => x.UserId == userId)
            .Select(x => x.BlogId)
            .ToListAsync();

        //! Find common blog IDs (intersection)
        var likedAndSavedBlogIds = likedBlogIds.Intersect(savedBlogIds).ToList();

        //! Fetch blog details
        var blogs = await _context.Blogs
            .Where(blog => likedAndSavedBlogIds.Contains(blog.Id))
            .Include(blog => blog.User)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        //! Prepare final DTO list
        var dtoList = new List<BlogDto>();
        foreach (var blog in blogs)
        {
            var dto = _mapper.Map<BlogDto>(blog);
            dto.LikeCount = blog.LikeCount;
            dto.IsLikedByCurrentUser = true;
            dto.SaveCount = blog.SaveCount;
            dto.IsSavedByCurrentUser = true;
            dtoList.Add(dto);
        }

        return dtoList;
    }

}
