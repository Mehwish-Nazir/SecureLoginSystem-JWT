using BCrypt.Net;

namespace BackeEndAuthentication.Helpers
{
    public static class PasswordHelpers
    {
        // BCrypt will auto-generate salt internally
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verifies the entered password against the stored hash
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}
