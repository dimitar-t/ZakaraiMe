﻿namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser<int>, IBaseEntity
    {
        public override int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Името трябва да e дългo между {2} и {1} символа.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Името трябва да e дългo между {2} и {1} символа.", MinimumLength = 3)]
        public string LastName { get; set; }
    }
}
