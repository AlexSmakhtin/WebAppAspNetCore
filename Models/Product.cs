using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Product: IEntity
    {
        [Required]
        public Guid Id { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
    }
}