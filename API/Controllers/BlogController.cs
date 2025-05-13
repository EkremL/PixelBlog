using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Exceptions;
using API.Extensions;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly BlogService _blogService;

        public BlogController(DataContext context, IMapper mapper, BlogService blogService)
        {
            _context = context;
            _mapper = mapper;
            _blogService = blogService;
        }
        //!CREATE BLOG
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBlog([FromBody] BlogCreateDto blogCreateDto)
        {
            // //!Get the currently authenticated user's ID from JWT claims.
            // // var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);  //*BEFORE EXTENSION METHOD
            // var userId = User.GetUserId(); //* after extension method
            // var user = await _context.Users.FindAsync(userId);
            // if(user == null) return Unauthorized();

            // //!if we weren't using automapper
            //     // var blog = new Blog
            //     // {
            //     //     Title = blogCreateDto.Title,
            //     //     Subtitle = blogCreateDto.Subtitle,
            //     //     Description = blogCreateDto.Description,
            //     //     ImageUrl = blogCreateDto.ImageUrl
            //     // };

            // //!(Automappar) Map BlogCreateDto to Blog entity
            // var blog = _mapper.Map<Blog>(blogCreateDto);

            // //? Add the new blog to the database
            // _context.Blogs.Add(blog);

            // //? Connect the blog with user.
            // blog.UserId = user.Id;

            // //?Update User Exp (+30 xp)
            // UpdateUserExperience(user,30);

            // //?update user blog count
            // user.BlogCount++;

            // await _context.SaveChangesAsync();

            // return CreatedAtAction(nameof(GetBlogById), new { id = blog.Id }, blog);


            // var userId = User.GetUserId();
            // if (userId == null) return Unauthorized();
            var userId = User.GetUserIdOrThrow();

            //!we moved to ClaimsPrincipalExtensions.cs and GetUserIdOrThrow() method is coming from there
            // if (userId == null)
            //     throw new ApiException(401, "Unauthorized", "Token is invalid or missing.");

            var result = await _blogService.CreateBlogAsync(blogCreateDto, userId);
            // if (result == null) return BadRequest("User not found or invalid blog data.");

            if (result == null)
                throw new ApiException(400, "Blog creation failed", "User not found or blog data is invalid.");
            return Ok(result);
        }

        //! GET ALL BLOGS
        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {

            // var blogs = await _context.Blogs.Include(b => b.User).ToListAsync();

            // if (!blogs.Any())
            //     throw new ApiException(404, "No blogs found", "There are currently no blogs available in the system.");

            // var mappedBlogs = _mapper.Map<List<BlogDto>>(blogs);

            // return Ok(mappedBlogs);

            int? userId = User.GetUserId(); //User who didn't login can see all blogs and likes
            var blogs = await _blogService.GetAllBlogsAsync(userId);
            return Ok(blogs);


        }

        //!GET SINGLE BLOG BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {

            var blog = await _context.Blogs.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
            // if (blog == null) return NotFound();

            if (blog == null)
                throw new ApiException(404, "Blog not found", $"No blog exists with ID = {id}.");

            var mappedBlog = _mapper.Map<BlogDto>(blog);
            return Ok(mappedBlog);
        }

        //!UPDATE BLOG
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogUpdateDto blogDto)
        {

            var blog = await _context.Blogs.FindAsync(id);
            // if (blog == null)
            //     return NotFound();

            if (blog == null)
                throw new ApiException(404, "Blog not found", $"Cannot update blog with ID = {id} because it doesn't exist.");

            _mapper.Map(blogDto, blog);

            blog.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //!DELETE BLOG
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog(int id)
        {

            var blog = await _context.Blogs.FindAsync(id);
            // if (blog == null)
            //     return NotFound();

            if (blog == null)
                throw new ApiException(404, "Blog not found", $"Cannot delete blog with ID = {id} because it doesn't exist.");

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //! Earn xp from reading per blog
        [HttpGet("{id}/read")]
        [Authorize]
        public async Task<IActionResult> ReadBlogById(int id)
        {
            //! we changed this method, blogservice will handle this job
            // var blog = await _context.Blogs.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
            // if (blog == null) return NotFound();

            // var userId = User.GetUserId();
            // var user = await _context.Users.FindAsync(userId);
            // if (user == null) return Unauthorized();

            // If the blog doesn't belong to the user, that is, if user reads someone else's blog, earn xp and increase the blog count.
            // if(blog.UserId != userId){
            //     blog.ViewCount++;
            //     UpdateUserExperience(user,10);
            // }

            // await _context.SaveChangesAsync();

            // var mappedBlog = _mapper.Map<BlogDto>(blog);

            // return Ok(mappedBlog);
            //!after blogservice
            var userId = User.GetUserIdOrThrow();
            // var userId = User.GetUserId();
            // if (userId == null) return Unauthorized();

            var blog = await _blogService.ReadBlogByIdAsync(id, userId);
            // if (blog == null) return NotFound();

            if (blog == null)
                throw new ApiException(404, "Blog not found", $"No blog exists with ID = {id}.");

            return Ok(blog);
        }

        //!GET MY BLOGS (get related User Blogs)
        [HttpGet("myblogs")]
        [Authorize]
        public async Task<IActionResult> GetMyBlogs()
        {

            // var userId = User.GetUserId();
            var userId = User.GetUserIdOrThrow();
            var blogs = await _context.Blogs.Where(b => b.UserId == userId).ToListAsync();

            if (blogs == null || !blogs.Any())
                throw new ApiException(404, "No blogs found", "You haven't written any blogs yet.");

            var mappedBlogs = _mapper.Map<List<BlogDto>>(blogs);

            return Ok(mappedBlogs);
        }
        //!LIKE BLOG 
        [HttpPost("{blogId}/like")]
        [Authorize]
        public async Task<IActionResult> ToggleLike(int blogId)
        {
            var userId = User.GetUserIdOrThrow();
            var result = await _blogService.ToggleLikeAsync(blogId, userId);
            return Ok(new { liked = result });
        }

        //!SAVE BLOG 
        [HttpPost("{blogId}/save")]
        [Authorize]
        public async Task<IActionResult> ToggleSave(int blogId)
        {
            var userId = User.GetUserIdOrThrow();
            var result = await _blogService.ToggleSaveAsync(blogId, userId);
            return Ok(new { saved = result });
        }

        //! GET BLOGS LIKED BY CURRENT USER
        [HttpGet("liked")]
        [Authorize]
        public async Task<IActionResult> GetLikedBlogs()
        {
            var userId = User.GetUserIdOrThrow();
            var result = await _blogService.GetLikedBlogsAsync(userId);
            return Ok(result);
        }
        //! GET BLOGS SAVED BY CURRENT USER
        [HttpGet("saved")]
        [Authorize]
        public async Task<IActionResult> GetSavedBlogs()
        {
            var userId = User.GetUserIdOrThrow();
            var result = await _blogService.GetSavedBlogsAsync(userId);
            return Ok(result);
        }

        //! GET BLOGS BOTH LIKED AND SAVED BY CURRENT USER
        [HttpGet("liked-and-saved")]
        [Authorize]
        public async Task<IActionResult> GetLikedAndSavedBlogs()
        {
            var userId = User.GetUserIdOrThrow();
            var result = await _blogService.GetLikedAndSavedBlogsAsync(userId);
            return Ok(result);
        }
    }
}
