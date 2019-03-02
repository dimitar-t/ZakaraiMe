namespace ZakaraiMe.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Journeys;
    using ZakaraiMe.Data.Entities.Implementations;
    using ZakaraiMe.Service.Contracts;

    public class JourneysController : BaseController<Journey, JourneyFormViewModel, JourneyListViewModel>
    {
        public JourneysController(IJourneyService journeyService, UserManager<User> userManager, IMapper mapper) : base(journeyService, userManager, mapper)
        {
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

            journey.StartPointX = viewModel.StartingPointX;
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
            throw new System.NotImplementedException();
        }
    }
}