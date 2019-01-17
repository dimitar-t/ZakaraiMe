namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Role : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Името трябва да бъде между {2} и {1} символа дълго.", MinimumLength = 3)]
        public string Name { get; set; }

        public virtual IEnumerable<UserRole> Users { get; set; } = new List<UserRole>();
    }
}
