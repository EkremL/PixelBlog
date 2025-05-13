using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

//! This service is responsible for generating JWT tokens for authenticated users.
public class JwtService
{
    private readonly IConfiguration _config;

    //! Inject IConfiguration to access appsettings.json (to retrieve JWT settings)
    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    //! This method generates a signed JWT token based on the given user information
    public string CreateToken(User user)
    {
        //! Step 1: Define the claims (data) to store inside the token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        //! Step 2: Create the security key from appsettings.json
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));

        //! Step 3: Define how the token will be signed (HMAC SHA512)
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //! Step 4: Set up token properties (claims, expiration, signing info, etc.)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"]!)),
            Issuer = _config["JwtSettings:Issuer"],
            Audience = _config["JwtSettings:Audience"],
            SigningCredentials = creds
        };

        //! Step 5: Generate the token using JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        //! Step 6: Return the token string to the client
        return tokenHandler.WriteToken(token);
    }
}
