namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity, new()
    {
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> filter = null);

        Task<TEntity> GetById(int id);

        Task Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task SaveAsync();
    }
}
