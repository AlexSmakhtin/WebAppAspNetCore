using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MySuperShop.ApiGateway.Configs;

public class JwtConfig
{
    [Required,MinLength(20)]
    public string SigningKey { get; set; } = "";
    
    [Required]
    public TimeSpan LifeTime { get; set; }
    
    [Required]
    public string Audience { get; set; } = "";
    
    [Required]
    public string Issuer { get; set; } = "";

    public byte[] SigningKeyBytes => Encoding.UTF8.GetBytes(SigningKey);
}
