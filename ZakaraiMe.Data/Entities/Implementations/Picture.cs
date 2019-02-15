namespace ZakaraiMe.Data.Entities.Implementations
{
    using System.ComponentModel.DataAnnotations;

    public class Picture
    {
        [Key]
        public string FileName { get; set; }
    }
}
