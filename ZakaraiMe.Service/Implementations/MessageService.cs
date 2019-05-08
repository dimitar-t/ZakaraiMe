namespace ZakaraiMe.Service.Implementations
{
    using Contracts;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository repo;

        public MessageService(IMessageRepository repo)
        {
            this.repo = repo;
        }

        public async Task CreateAsync(Message message)
        {
            await repo.CreateAsync(message);
        }

        public IEnumerable<Message> GetAll(Func<Message, bool> filter = null)
        {
            return repo.GetAll(filter);
        }

        public Message GetEntity(string text, int senderId, int receiverId)
        {
            return new Message
            {
                Text = text,
                SenderId = senderId,
                ReceiverId = receiverId
            };
        }

        public async Task SaveAsync()
        {
            await repo.SaveAsync();
        }
    }
}
