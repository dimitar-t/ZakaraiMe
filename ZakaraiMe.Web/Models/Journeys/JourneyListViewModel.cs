namespace ZakaraiMe.Web.Models.Journeys
{
    using Cars;
    using Common.Mapping;
    using Data.Entities.Implementations;
    using System;
    using System.Collections.Generic;
    using Users;

    public class JourneyListViewModel : ListViewModel, IMapFrom<Journey>
    {
        public decimal StartPointX { get; set; }

        public decimal StartPointY { get; set; }

        public decimal EndPointX { get; set; }

        public decimal EndPointY { get; set; }

        public decimal Price { get; set; }

        public int Seats { get; set; }

        public CarListViewModel Car { get; set; }

        public int CarId { get; set; }

        public UserListViewModel Driver { get; set; }

        public int DriverId { get; set; }

        public IEnumerable<UserJourney> Passengers { get; set; } = new List<UserJourney>();

        public DateTime SetOffTime { get; set; }
    }
}
