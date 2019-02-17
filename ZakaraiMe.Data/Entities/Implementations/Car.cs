namespace ZakaraiMe.Data.Entities.Implementations
{
    using System.ComponentModel.DataAnnotations;
    using ZakaraiMe.Data.Entities.Contracts;

    public class Car : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Марката трябва да e дългo между {2} и {1} символа.", MinimumLength = 3)]
        public string Make { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Моделът трябва да e дългo между {2} и {1} символа.", MinimumLength = 3)]
        public string Model { get; set; }

        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual Picture Picture { get; set; }

        [Required]
        public string PictureFileName { get; set; }
    }
}
