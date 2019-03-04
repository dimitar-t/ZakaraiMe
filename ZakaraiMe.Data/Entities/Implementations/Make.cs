namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity with properties for car makes (i.e Mercedes, BMW).
    /// </summary>
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
