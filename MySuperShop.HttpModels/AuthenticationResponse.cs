using System.ComponentModel.DataAnnotations;

namespace MySuperShop.HttpModels;

public class AuthenticationResponse
{
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Jwt { get; set; }

    public AuthenticationResponse(string email, string jwt)
    {
        Email = email;
        Jwt = jwt;
    }
}