namespace ZakaraiMe.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Journeys;
    using Data.Entities.Implementations;
    using Service.Contracts;
    using Models.Cars;
    using Microsoft.AspNetCore.Authorization;
    using Common;

    public class JourneysController : BaseController<Journey, JourneyFormViewModel, JourneyListViewModel>
    {
        private readonly ICarService carService;

        public JourneysController(IJourneyService journeyService, UserManager<User> userManager, ICarService carService, IMapper mapper) : base(journeyService, userManager, mapper)
        {
            this.carService = carService;
        }

        protected override string ItemName { get; set; } = "пътуване";

        public IActionResult MapBox()
        {
            return View();
        }

        protected override async Task<Journey> GetEntityAsync(JourneyFormViewModel viewModel, int id)
        {
            Journey journey = await service.GetByIdAsync(id);

            if (journey == null) // If journey doesn't exist (on create action)
            {
                journey = new Journey();

                // Properties which can't be updated
                journey.DriverId = GetCurrentUserAsync().Result.Id;
            }

            journey.StartPointX = viewModel.StartPointX;
            journey.StartPointY = viewModel.StartPointY;
            journey.EndPointX = viewModel.EndPointX;
            journey.EndPointY = viewModel.EndPointY;
            journey.Price = viewModel.Price;
            journey.SetOffTime = viewModel.SetOffTime;
            journey.CarId = viewModel.CarId;

            if (viewModel.Seats >= journey.Passengers.Count()) // Make sure there's a seat for every passenger
                journey.Seats = viewModel.Seats;

            return journey;
        }

        protected override JourneyFormViewModel SendFormData(Journey item, JourneyFormViewModel viewModel)
        {
            int driverId;

            if (item == null) // This means that the journey is to be created            
                driverId = GetCurrentUserAsync().Result.Id;
            else
                driverId = item.DriverId;
            
            viewModel = viewModel ?? new JourneyFormViewModel();

            if (viewModel.DriverCars.Count() == 0)
                viewModel.DriverCars = mapper.Map<IEnumerable<Car>, IEnumerable<CarListViewModel>>(carService.GetAll(c => c.OwnerId == driverId));

            return viewModel;
        }

        [Authorize(Roles = CommonConstants.DriverRole)]
        public override async Task<IActionResult> Create(JourneyFormViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        [Authorize(Roles = CommonConstants.DriverRole)]
        public override IActionResult Create()
        {
            return base.Create();
        }
    }
}