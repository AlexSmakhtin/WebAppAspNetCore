using System.ComponentModel.DataAnnotations;

namespace MySuperShop.HttpModels;

public class RegistrationResponse
{
    [StringLength(100, MinimumLength = 2)] public string Name { get; set; } = null!;

    [EmailAddress] public string Email { get; set; } = null!;

    public RegistrationResponse(string name, string email)
    {
        Name = name;
        Email = email;
    }
}