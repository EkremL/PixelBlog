using API.Data;
using API.DTOs;
using API.Entities;
using API.Exceptions;
using API.Extensions;
using API.Helpers;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        private readonly JwtService _jwtService;

        private readonly UserService _userService;

        public AuthController(DataContext context, IMapper mapper, JwtService jwtService, UserService userService)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
            _userService = userService;
        }
        //!REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            //?we moved this logic to UserService (Separation of Concerns)
            // var userExist = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
            // //!before error handling            
            // // if (userExist)
            // //     return BadRequest("User already registered!");
            // //!after error handling(ApiException.cs)
            // if (userExist)
            //     throw new ApiException(400, "User already registered!", "A user with the same email address already exists.");

            // //!hashing pw
            // PasswordHelper.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // //! DTO → Entity (With AutoMapper)
            // var user = _mapper.Map<User>(registerDto);

            // //! Manuel fields (Not Exist in Dto)
            // user.PasswordHash = passwordHash;
            // user.PasswordSalt = passwordSalt;
            // user.Role = UserRole.User;
            // user.Level = 1;
            // user.Experience = 0;
            // user.ExpToNextLevel = 100;
            // user.CreatedAt = DateTime.UtcNow;
            // user.UpdatedAt = DateTime.UtcNow;

            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();

            //?Now, this comes from Service (clean code :) )
            await _userService.RegisterAsync(registerDto);
            return Ok("Registration successful!");
        }
        //!LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            //?we moved this logic to UserService  (Separation of Concerns)
            // var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.UsernameOrEmail || u.Username == loginDto.UsernameOrEmail);

            // // if (user == null)
            // //     return Unauthorized();
            // if (user == null)
            //     throw new ApiException(401, "Invalid credentials", "No user found with the given email or username.");

            // var isPasswordValid = PasswordHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!);

            // // if (!isPasswordValid)
            // //     return Unauthorized();

            // if (!isPasswordValid)
            //     throw new ApiException(401, "Invalid credentials", "Incorrect password provided for this user.");

            // var token = _jwtService.CreateToken(user);
            //?Now, this comes from Service (clean code :) )

            var token = await _userService.LoginAsync(loginDto);
            return Ok(new { token });
        }

        //!Get Current User 

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            //?we moved this logic to UserService (Separation of Concerns)
            // var userId = User.GetUserId();

            // var user = await _context.Users.Include(u => u.Blogs).FirstOrDefaultAsync(u => u.Id == userId);

            // // if (user == null) return Unauthorized();

            // if (user == null)
            //     throw new ApiException(401, "Unauthorized", "The user was not found or the token is invalid.");

            // //map userdto and dynamically calculate actual blog count
            // var mappedUser = _mapper.Map<UserDto>(user);
            // mappedUser.BlogCount = user.Blogs.Count;

            //?Now, this comes from Service (clean code :) )

            var userId = User.GetUserIdOrThrow(); // helper extension used
            var user = await _userService.GetCurrentUserAsync(userId);
            return Ok(user);
        }

    }
}