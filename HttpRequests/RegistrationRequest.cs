using System.ComponentModel.DataAnnotations;

namespace HttpModels
{
    public class RegistrationRequest
    {
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
