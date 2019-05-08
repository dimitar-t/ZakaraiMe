namespace ZakaraiMe.Data.Repositories.Implementations
{
    using Contracts;
    using Entities.Implementations;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MessageRepository : IMessageRepository
    {
        private ZakaraiMeContext context;
        private DbSet<Message> dbSet;

        public MessageRepository(ZakaraiMeContext context)
        {
            this.context = context;
            dbSet = context.Set<Message>();
        }

        public async Task CreateAsync(Message message)
        {
            await dbSet.AddAsync(message);
        }

        public IEnumerable<Message> GetAll(Func<Message, bool> filter = null)
        {
            if (filter == null)
            {
                return dbSet.ToList();
            }
            else
            {
                return dbSet.Where(filter);
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
