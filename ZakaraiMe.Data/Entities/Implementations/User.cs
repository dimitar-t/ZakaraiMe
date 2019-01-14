namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Потребителкото име трябва да бъде между {2} и {1} символа дълго.", MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Паролата трябва да бъде между {2} и {1} символа дълга.", MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Собственото име трябва да бъде между {2} и {1} символа дълго.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Фамилното име трябва да бъде между {2} и {1} символа дълго.", MinimumLength = 3)]
        public string LastName { get; set; }

        public virtual IEnumerable<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
