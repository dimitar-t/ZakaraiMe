namespace ZakaraiMe.Web.Models.Journeys
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Common.Mapping;
    using Data.Entities.Implementations;
    using Models.Cars;

    public class JourneyFormViewModel : FormViewModel, IMapFrom<Journey>, IHaveCustomMapping
    {
        public decimal StartingPointX { get; set; }

        public decimal StartPointY { get; set; }

        public decimal EndPointX { get; set; }

        public decimal EndPointY { get; set; }

        public decimal Price { get; set; }

        public int Seats { get; set; }

        public int CarId { get; set; }

        public DateTime SetOffTime { get; set; }

        public IEnumerable<CarListViewModel> DriverCars { get; set; } = new List<CarListViewModel>();

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Journey, JourneyFormViewModel>()
                .ForMember(j => j.DriverCars, cfg => cfg.Ignore());
        }
    }
}
