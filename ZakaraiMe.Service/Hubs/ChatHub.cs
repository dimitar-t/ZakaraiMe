namespace ZakaraiMe.Service.Hubs
{
    using Contracts;
    using Data.Entities.Implementations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        private readonly UserManager<User> userManager;
        private readonly IMessageService messageService;
        private const string Error = "Възникна грешка при изпращането на това съобщение.";

        public ChatHub(UserManager<User> userManager, IMessageService messageService)
        {
            this.userManager = userManager;
            this.messageService = messageService;
        }

        public async Task SendMessage(string receiverIdString, string text)
        {
            string senderIdString = userManager.GetUserId(Context.User);

            User receiver = await userManager.FindByIdAsync(receiverIdString);
            
            IClientProxy receiverProxy = Clients.User(receiverIdString);

            if (receiver == null) // if receiver doesn't exist
            {
                return; // return return; kek
            }

            int receiverId;
            int senderId;

            bool success = true;
            success = int.TryParse(receiverIdString, out receiverId);
            success = int.TryParse(senderIdString, out senderId);            

            if (!success) // if the passed id is invalid
            {
                return;
            }            

            Message message = messageService.GetEntity(text, senderId, receiverId);
            await messageService.CreateAsync(message);
            await messageService.SaveAsync();

            await receiverProxy.SendAsync("ReceiveMessage", senderIdString, text, message.Id);
        }
    }
}
