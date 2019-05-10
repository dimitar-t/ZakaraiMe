namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Data.Entities.Implementations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Journeys;
    using Models.Users;
    using Service.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ZakaraiMe.Web.Models.Messages;

    public class ChatController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMessageService messageService;
        private readonly IJourneyService journeyService;
        private readonly IMapper mapper;

        public ChatController(UserManager<User> userManager, IMessageService messageService, IJourneyService journeyService, IMapper mapper)
        {
            this.userManager = userManager;
            this.messageService = messageService;
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
                return Json(mapper.Map<IEnumerable<User>, IEnumerable<UserChatViewModel>>(journey.Passengers.Select(p => p.User)));
            }
            else if (journey.Passengers.Any(p => p.UserId == currentUser.Id)) //return the driver only if the sender is one of the passengers
            {
                return Json(mapper.Map<User,UserChatViewModel>(journey.Driver));
            }

            return null;
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetMessages(int receiverId, int lastMessageId)
        {
            User currentUser = await userManager.GetUserAsync(User);

            if (lastMessageId == 0)
                lastMessageId = int.MaxValue;

            IEnumerable<Message> messages = messageService
                                            .GetAll(m => (m.Id < lastMessageId))
                                            .Where(m => (m.ReceiverId == receiverId && m.SenderId == currentUser.Id) || (m.ReceiverId == currentUser.Id && m.SenderId == receiverId))
                                            .OrderByDescending(m => m.Id)
                                            .Take(10);

            return Json(mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messages));
        }
    }
}