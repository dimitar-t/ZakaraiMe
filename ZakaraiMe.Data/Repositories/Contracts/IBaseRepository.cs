namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> filter = null);

        Task<TEntity> GetByIdAsync(int id);

        Task CreateAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task SaveAsync();
    }
}
