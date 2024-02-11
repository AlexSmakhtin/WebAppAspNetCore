using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Services;

namespace ApiGateway.Services.Implementations;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly IPasswordHasher<Account> _passwordHasher;

    public PasswordHasherService(IOptions<PasswordHasherOptions> options)
    {
        _passwordHasher = new PasswordHasher<Account>(options);
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
        return result != PasswordVerificationResult.Failed;
    }
}