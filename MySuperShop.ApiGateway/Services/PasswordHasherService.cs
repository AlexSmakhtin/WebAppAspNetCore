using Microsoft.AspNetCore.Identity;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Services;

namespace MySuperShop.ApiGateway.Services;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly IPasswordHasher<Account> _passwordHasher;

    public PasswordHasherService(IPasswordHasher<Account> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(Account account, string password)
    {
        return _passwordHasher.HashPassword(account, password);
    }

    public bool VerifyPassword(Account account, string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(account, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}