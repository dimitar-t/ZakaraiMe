namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Make : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual IEnumerable<Model> Models { get; set; } = new List<Model>();
    }
}
