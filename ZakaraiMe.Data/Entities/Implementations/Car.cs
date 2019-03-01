namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Car : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Colour { get; set; }

        public virtual Model Model { get; set; }
        
        public int ModelId { get; set; }

        public virtual User Owner { get; set; }

        public int OwnerId { get; set; }

        public virtual Picture Picture { get; set; }

        [Required]
        public string PictureFileName { get; set; }

        public virtual IEnumerable<Journey> Journeys { get; set; } = new List<Journey>();
    }
}
