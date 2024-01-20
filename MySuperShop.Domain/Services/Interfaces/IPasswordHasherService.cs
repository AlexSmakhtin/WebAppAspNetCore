using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Services;

public interface IPasswordHasherService
{
    string HashPassword(Account account, string password);
    bool VerifyPassword(Account account, string hashedPassword, string providedPassword);
}