namespace ZakaraiMe.Web.Models.Users
{
    using AutoMapper;
    using Data.Entities.Implementations;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ZakaraiMe.Common.Mapping;

    public class UserListViewModel : ListViewModel, IMapFrom<User>, IHaveCustomMapping
    {        
        public string ProfilePictureFileName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public virtual IEnumerable<Car> Cars { get; set; } = new List<Car>(); // TODO: Change from List<Car> to List<CarListViewModel.

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<User, UserListViewModel>()
                .ForMember(u => u.Roles, cfg => cfg.Ignore());
        }
    }
}
