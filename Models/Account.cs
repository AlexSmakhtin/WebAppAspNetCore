using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Account : IEntity
    {
        [Required]
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
