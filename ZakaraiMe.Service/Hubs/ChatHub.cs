namespace ZakaraiMe.Service.Hubs
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    using ZakaraiMe.Data.Entities.Implementations;

    public class ChatHub : Hub
    {
        private readonly UserManager<User> userManager;

        public ChatHub(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            string senderId = userManager.GetUserId(Context.User);
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
        }
    }
}
