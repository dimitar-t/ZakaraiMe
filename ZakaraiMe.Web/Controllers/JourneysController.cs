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
        [HttpGet]
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
                return RedirectToHome();
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
                return RedirectToHome();
            }

            journeyService.JoinJourney(journey, currentUserId);
            await service.SaveAsync();

            TempData.AddSuccessMessage(WebConstants.SuccessfulJoin);
            return RedirectToHome();
        }

        [Authorize]
        [HttpGet]
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

            if (!journey.Passengers.Any(p => p.UserId == currentUserId)) // Check if the user is part of the journey
            {
                TempData.AddErrorMessage(WebConstants.NotPartOfTheJourney);
                return RedirectToHome();
            }

            journeyService.LeaveJourney(journey, currentUserId);
            await service.SaveAsync();

            TempData.AddWarningMessage(WebConstants.WarningLeaveJourney);
            return RedirectToHome();
        }

        public async Task<IActionResult> Search(JourneySearchViewModel searchParams)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToHome();
            }

            if (searchParams == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToHome();
            }

            if (searchParams.StartPointX == 0 || searchParams.StartPointY == 0 || searchParams.EndPointX == 0 || searchParams.EndPointY == 0)
            {
                TempData.AddErrorMessage(WebConstants.MissingJourney);
                return RedirectToHome();
            }

            if(searchParams.SetOffTime < DateTime.Now)
            {
                TempData.AddErrorMessage(WebConstants.JourneyInThePast);
                return RedirectToHome();
            }

            List<JourneyListViewModel> result = new List<JourneyListViewModel>();
            FindMatches(searchParams, ref result);

            return View("Results", result);
        }

        private void FindMatches(JourneySearchViewModel searchParams, ref List<JourneyListViewModel> result)
        {
            //Set the search parameters
            double startSearchLat = (double)searchParams.StartPointX;
            double startSearchLon = (double)searchParams.StartPointY;
            double endSearchLat = (double)searchParams.EndPointX;
            double endSearchLon = (double)searchParams.EndPointY;
            DateTime searchDate = searchParams.SetOffTime;
            int searchRadius = searchParams.Radius;
            int currentUserId = GetCurrentUserAsync().Result.Id;

            IEnumerable<Journey> journeys = service.GetAll(j => j.SetOffTime.Date == searchDate.Date && j.Seats > j.Passengers.Count && j.DriverId != currentUserId);

            foreach (Journey journey in journeys)
            {
                if (journeyService.IsMatch(startSearchLat, startSearchLon, endSearchLat, endSearchLon, searchRadius, searchDate, journey))
                {
                    result.Add(mapper.Map<Journey, JourneyListViewModel>(journey));
                }
            }
        }
    }
}