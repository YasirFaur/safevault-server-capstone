// Use the BCrypt library for secure password hashing
using BCrypt.Net;
namespace SafeVault
{
    public static class PasswordHasher
    {
        // This function converts a plain password into a secure, encrypted text
        public static string HashPassword(string password)
        {
            // Generate and return the secure hash using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // This function checks if the entered password matches the stored encrypted hash
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Compare the plain text password with the safe hash and return true or false
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
