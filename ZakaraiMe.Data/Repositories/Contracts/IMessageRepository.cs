namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Implementations;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessageRepository
    {
        Task CreateAsync(Message message);

        IEnumerable<Message> GetAll(Func<Message, bool> filter = null);

        void Delete(IEnumerable<Message> messages);

        Task SaveAsync();
    }
}
