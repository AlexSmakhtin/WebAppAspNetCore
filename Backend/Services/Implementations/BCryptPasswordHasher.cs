using Domain.Services.Interfaces;

namespace Backend.Services.Implementations
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string GenerateHash(string password, string? salt = null)
        {
            if (salt == null)
                return BCrypt.Net.BCrypt.HashPassword(password);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
