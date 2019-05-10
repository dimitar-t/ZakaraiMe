namespace ZakaraiMe.Service.Implementations
{
    using Common;
    using Contracts;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class JourneyService : BaseService<Journey>, IJourneyService
    {
        private readonly ICarService carService;
        private readonly UserManager<User> userManager;

        public JourneyService(IJourneyRepository repository, ICarService carService, UserManager<User> userManager) : base(repository)
        {
            this.carService = carService;
            this.userManager = userManager;
        }

        public override async Task<bool> ForeignPropertiesExistAsync(Journey item, User currentUser)
        {
            Car car = await carService.GetByIdAsync(item.CarId);

            if (car == null)
                return false;

            if (item.DriverId != car.OwnerId) // returns false if the driver doesn't own the car (i.e. the user changed the <select> tag)
                return false;

            return true;
        }

        public override async Task<IEnumerable<Journey>> GetFilteredItemsAsync(User currentUser)
        {
            if (await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
                return GetAll();

            return GetAll(j => j.DriverId == currentUser.Id || j.Passengers.Any(p => p.UserId == currentUser.Id));
        }

        public override bool IsItemDuplicate(Journey item)
        {
            return GetAll(j => j.CarId == item.CarId
                            && j.DriverId == item.DriverId
                            && j.SetOffTime == item.SetOffTime
                            && j.Id != item.Id)
                    .Any();
        }

        public override async Task<bool> IsUserAuthorizedAsync(Journey item, User currentUser)
        {
            if (await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
                return true;
            
            return item.DriverId == currentUser.Id;
        }

        public void JoinJourney(Journey journey, int currentUserId)
        {
            journey.Passengers
                .Add(new UserJourney
            {
                UserId = currentUserId
            });
        }

        public void LeaveJourney(Journey journey, int currentUserId)
        {
            UserJourney userJourneyToDelete = journey.Passengers.FirstOrDefault(p => p.UserId == currentUserId);

            journey.Passengers
                .Remove(userJourneyToDelete);
        }

        public bool IsMatch(double startSearchLat, double startSearchLon, double endSearchLat, double endSearchLon, int radius, DateTime date, Journey journey)
        {
            //Check if journey's start and end point are in the given radius
            if (GetDistance(startSearchLat, startSearchLon, (double)journey.StartPointX, (double)journey.StartPointY) < radius)
            {
                if (GetDistance(endSearchLat, endSearchLon, (double)journey.EndPointX, (double)journey.EndPointY) < radius)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Delete(Journey item)
        {
            foreach (UserJourney passenger in item.Passengers.ToList())
            {
                this.LeaveJourney(item, passenger.User.Id);
            }

            base.Delete(item);
        }

        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            int R = 6371; // Radius of the earth in km
            double dLat = ToRadians(lat2 - lat1);  // deg2rad below
            double dLon = ToRadians(lon2 - lon1);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c * 1000; // Distance in meters
            return d;
        }

        private double ToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
