using System.Security.Cryptography;
using System.Text;

namespace API.Helpers;

public static class PasswordHelper
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(); // Secure Algorithm  (we used "using" for after this process, this call hmac.Dispose()) and this way also modern c# way

        passwordSalt = hmac.Key; // Random salt
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt); // Use same salt

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash); // Do the hashes match? 
    }
}
