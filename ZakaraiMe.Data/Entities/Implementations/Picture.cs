namespace ZakaraiMe.Data.Entities.Implementations
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity with property for storing the file name of a file in the file system.
    /// </summary>
    public class Picture
    {
        [Key]
        public string FileName { get; set; }
    }
}
