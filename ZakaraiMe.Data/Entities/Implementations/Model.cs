namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity with properties for car model (i.e. E-Class, 316i)
    /// </summary>
    public class Model : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual Make Make { get; set; }
        
        public int MakeId { get; set; }

        public virtual IEnumerable<Car> Cars { get; set; } = new List<Car>();
    }
}
