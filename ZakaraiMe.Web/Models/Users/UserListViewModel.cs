namespace ZakaraiMe.Web.Models.Users
{
    using AutoMapper;
    using Data.Entities.Implementations;
    using System.Collections.Generic;
    using Common.Mapping;

    public class UserListViewModel : ListViewModel, IMapFrom<User>, IHaveCustomMapping
    {        
        public string ProfilePictureFileName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<User, UserListViewModel>()
                .ForMember(u => u.Roles, cfg => cfg.Ignore());
        }
    }
}
