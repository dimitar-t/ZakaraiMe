namespace ZakaraiMe.Web.Models.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Entities.Implementations;

    public class UserListViewModel : ListViewModel
    {
        [Required]
        public string ProfilePictureFileName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public virtual IEnumerable<Car> Cars { get; set; } = new List<Car>(); // TODO: Change from List<Car> to List<CarListViewModel.        
    }
}
