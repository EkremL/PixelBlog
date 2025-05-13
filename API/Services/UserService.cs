using API.Data;
using API.DTOs;
using API.Entities;
using API.Exceptions;
using API.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Services;

namespace API.Services;

//! This service handles all user-related operations: registration, login, current user retrieval
public class UserService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly JwtService _jwtService;

    public UserService(DataContext context, IMapper mapper, JwtService jwtService)
    {
        _context = context;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    //! Handles user registration
    public async Task RegisterAsync(UserRegisterDto registerDto)
    {
        var userExist = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
        if (userExist)
            throw new ApiException(400, "User already registered!", "A user with the same email address already exists.");

        PasswordHelper.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.Role = UserRole.User;
        user.Level = 1;
        user.Experience = 0;
        user.ExpToNextLevel = 100;
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    //! Handles user login and token generation
    public async Task<string> LoginAsync(UserLoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == loginDto.UsernameOrEmail || u.Username == loginDto.UsernameOrEmail);

        if (user == null)
            throw new ApiException(401, "Invalid credentials", "No user found with the given email or username.");

        var isPasswordValid = PasswordHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!);
        if (!isPasswordValid)
            throw new ApiException(401, "Invalid credentials", "Incorrect password provided for this user.");

        return _jwtService.CreateToken(user);
    }

    //! Returns the currently authenticated user info
    public async Task<UserDto> GetCurrentUserAsync(int userId)
    {
        var user = await _context.Users.Include(u => u.Blogs).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new ApiException(401, "Unauthorized", "The user was not found or the token is invalid.");

        var mappedUser = _mapper.Map<UserDto>(user);
        mappedUser.BlogCount = user.Blogs.Count;
        return mappedUser;
    }
}
