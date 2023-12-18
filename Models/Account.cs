﻿using System;
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
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$")]
        public string EmailAddress { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
