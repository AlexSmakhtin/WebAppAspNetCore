using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Services;

public interface IPasswordHasherService
{
    string HashPassword( string password);
    bool VerifyPassword( string hashedPassword, string providedPassword);
}