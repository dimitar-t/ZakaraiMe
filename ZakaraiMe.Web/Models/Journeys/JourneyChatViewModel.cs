namespace ZakaraiMe.Web.Models.Journeys
{
    using AutoMapper;
    using Cars;
    using Common.Mapping;
    using Data.Entities.Implementations;
    using System;
    using Users;

    public class JourneyChatViewModel : IMapFrom<Journey>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int Seats { get; set; }

        public CarListViewModel Car { get; set; }

        public int CarId { get; set; }

        public UserListViewModel Driver { get; set; }

        public int DriverId { get; set; }

        public int PassengersCount { get; set; }

        public DateTime SetOffTime { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<Journey, JourneyChatViewModel>()
                .ForMember(j => j.PassengersCount, cfg => cfg.MapFrom(e => e.Passengers.Count));
        }
    }
}
