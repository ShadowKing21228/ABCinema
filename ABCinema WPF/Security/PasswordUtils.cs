using Microsoft.AspNetCore.Identity;

namespace ABCinema_WPF.Security;

public static class PasswordUtils
{
    private static readonly PasswordHasher<string> hasher = new();

    public static string HashPassword(string password)
    {
        return hasher.HashPassword("", password);   
    }

    public static bool VerifyPassword(string password, string hashedPassword) {
        var result = hasher.VerifyHashedPassword("", hashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}