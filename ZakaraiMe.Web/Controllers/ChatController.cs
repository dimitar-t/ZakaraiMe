namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ZakaraiMe.Data.Entities.Implementations;
    using ZakaraiMe.Service.Contracts;
    using ZakaraiMe.Web.Models.Journeys;
    using ZakaraiMe.Web.Models.Users;

    public class ChatController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IJourneyService journeyService;
        private readonly IMapper mapper;

        public ChatController(UserManager<User> userManager, IJourneyService journeyService, IMapper mapper)
        {
            this.userManager = userManager;
            this.journeyService = journeyService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [Route("/chatbox")]
        public IActionResult Index()
        {
            string currentUsername = User.Identity.Name;

            IList<Journey> journeys = journeyService.GetAll(j => j.Driver.UserName == currentUsername || j.Passengers.Any(p => p.User.UserName == currentUsername))
                                                    .OrderByDescending(j => j.SetOffTime)
                                                    .ToList();

            IList<JourneyChatViewModel> mappedJourneys = mapper.Map<IList<Journey>, IList<JourneyChatViewModel>>(journeys);

            return View(mappedJourneys);
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetContacts(int journeyId)
        {
            User currentUser = await userManager.GetUserAsync(User);
            Journey journey = await journeyService.GetByIdAsync(journeyId);

            if (journey == null)
            {
                return null;
            }

            // return all the passengers if the sender of the message is the driver of the journey
            if (journey.DriverId == currentUser.Id)
            {
                //TODO: Change the destination model to a suitable one.
                //TODO: Test the case when there are many people in the journey.
                return Json(mapper.Map<IEnumerable<User>, IEnumerable<UserListViewModel>>(journey.Passengers.Select(p => p.User)));
            }
            else if (journey.Passengers.Any(p => p.UserId == currentUser.Id)) //return the driver only if the sender is one of the passengers
            {
                return Json(mapper.Map<User,UserListViewModel>(journey.Driver)); //TODO: Change the destination model to a suitable one.
            }

            return null;
        }
    }
}