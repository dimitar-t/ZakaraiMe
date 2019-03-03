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
    using ZakaraiMe.Web.Infrastructure.Extensions;
    using System;

    public class JourneysController : BaseController<Journey, JourneyFormViewModel, JourneyListViewModel>
    {
        private readonly ICarService carService;
        private readonly IJourneyService journeyService;

        public JourneysController(IJourneyService journeyService, UserManager<User> userManager, ICarService carService, IMapper mapper) : base(journeyService, userManager, mapper)
        {
            this.carService = carService;
            this.journeyService = journeyService;
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

        public override async Task<IActionResult> Delete(int id)
        {
            Journey journey = await journeyService.GetByIdAsync(id);

            if (journey == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return NotFound();
            }

            if (journey.Passengers.Count() > 0)
            {
                TempData.AddErrorMessage(WebConstants.JourneyHasPassengers);
                return RedirectToHome();
            }

            return await base.Delete(id);
        }

        [Authorize]
        public async Task<IActionResult> Join(int id)
        {
            Journey journey = await journeyService.GetByIdAsync(id);

            if (journey == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return NotFound();
            }

            if (journey.Passengers.Count() == journey.Seats) // Check if there are available seats
            {
                TempData.AddErrorMessage(WebConstants.FullCar);
                string lastSearch = Request.Headers["Referer"].ToString();
                return Redirect(lastSearch);
            }

            if (journey.SetOffTime < DateTime.UtcNow)
            {
                TempData.AddErrorMessage(WebConstants.JourneyInThePast);
                return RedirectToHome();
            }

            int currentUserId = GetCurrentUserAsync().Result.Id;

            if (journey.Passengers.Any(p => p.UserId == currentUserId)) // Check if the user isn't already part of the journey
            {
                TempData.AddErrorMessage(WebConstants.AlreadyJoined);
                return RedirectToHome();
            }

            if (journey.DriverId == currentUserId) // Check if the user isn't the driver of the journey
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                string lastSearch = Request.Headers["Referer"].ToString();
                return Redirect(lastSearch);
            }

            journeyService.JoinJourney(journey, currentUserId);
            await service.SaveAsync();

            TempData.AddSuccessMessage(WebConstants.SuccessfulJoin);
            return RedirectToHome();
        }

        [Authorize]
        public async Task<IActionResult> Leave(int id)
        {
            Journey journey = await journeyService.GetByIdAsync(id);

            if (journey == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return NotFound();
            }

            if (journey.SetOffTime < DateTime.UtcNow)
            {
                TempData.AddErrorMessage(WebConstants.JourneyInThePast);
                return RedirectToHome();
            }

            int currentUserId = GetCurrentUserAsync().Result.Id;

            if (!journey.Passengers.Any(p => p.UserId == currentUserId)) // Check if the user part of the journey
            {
                TempData.AddErrorMessage(WebConstants.AlreadyJoined);
                return RedirectToHome();
            }

            journeyService.LeaveJourney(journey, currentUserId);
            await service.SaveAsync();

            TempData.AddWarningMessage(WebConstants.WarningLeaveJourney);
            return RedirectToHome();
        }
    }
}