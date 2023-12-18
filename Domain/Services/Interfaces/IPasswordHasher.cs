namespace Domain.Services.Interfaces
{
    public interface IPasswordHasher
    {
        string GenerateHash(string password, string? salt = null);
        bool VerifyPassword(string password, string hash);
    }
}
