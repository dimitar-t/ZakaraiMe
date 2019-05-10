namespace ZakaraiMe.Service.Implementations
{
    using Common;
    using Contracts;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CarService : BaseService<Car>, ICarService
    {
        private readonly UserManager<User> userManager;
        private readonly IJourneyService journeyService;
        private new readonly ICarRepository repository;

        public CarService(ICarRepository repository, UserManager<User> userManager, IJourneyService journeyService) : base(repository)
        {
            this.userManager = userManager;
            this.repository = repository;
            this.journeyService = journeyService;
        }

        public override async Task<bool> ForeignPropertiesExistAsync(Car item, User currentUser)
        {
            Model carModel = await repository.GetModelAsync(item.ModelId);

            if (carModel == null)
                return false;

            return true;
        }

        public override async Task<IEnumerable<Car>> GetFilteredItemsAsync(User currentUser)
        {
            if (await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
                return GetAll();

            return GetAll(c => c.OwnerId == currentUser.Id);
        }

        public override bool IsItemDuplicate(Car item)
        {
            return GetAll(c => c.OwnerId == item.OwnerId
                            && c.Colour == item.Colour
                            && c.ModelId == item.ModelId
                            && c.Id != item.Id)
                    .Any();
        }

        public override async Task<bool> IsUserAuthorizedAsync(Car item, User currentUser)
        {
            if (await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
                return true;

            return item.OwnerId == currentUser.Id;
        }

        public async Task<IList<Make>> GetMakesAsync()
        {
            return await repository.GetAllMakesAsync();
        }

        public async Task<IList<Model>> GetModelsAsync(int makeId)
        {
            return await repository.GetAllModelsAsync(makeId);
        }

        public override void Delete(Car item)
        {
            foreach (Journey journey in item.Journeys.ToList())
            {
                journeyService.Delete(journey);
            }

            base.Delete(item);
        }
    }
}
