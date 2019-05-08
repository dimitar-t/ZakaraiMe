namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessageService
    {
        Task CreateAsync(Message message);

        IEnumerable<Message> GetAll(Func<Message, bool> filter = null);

        Message GetEntity(string text, int senderId, int receiverId);

        Task SaveAsync();
    }
}
