using System.ComponentModel.DataAnnotations;

namespace MySuperShop.HttpModels;

public class AuthenticationResponse
{
    [EmailAddress]
    public string Email { get; set; } = null!;

    public AuthenticationResponse(string email)
    {
        Email = email;
    }
}