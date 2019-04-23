namespace ZakaraiMe.Service.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            var identity = Context.User.Identity;
            await Clients.User(userId).SendAsync("ReceiveMessage", identity.Name, message);
        }
    }
}
