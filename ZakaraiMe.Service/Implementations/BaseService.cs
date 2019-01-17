namespace ZakaraiMe.Service.Implementations
{
    using Contracts;
    using Data.Entities.Contracts;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class Service<TEntity> : IBaseService<TEntity> where TEntity : class, IBaseEntity
    {
        protected IBaseRepository<TEntity> repository;

        public Service(IBaseRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public abstract Task<IEnumerable<TEntity>> GetFilteredItemsAsync(User currentUser);

        public abstract bool IsItemDuplicate(TEntity item);

        public abstract bool IsUserAuthorized(TEntity item, User currentUser);

        public abstract Task<bool> ForeignPropertiesExistAsync(TEntity item);

        public virtual void Delete(TEntity item)
        {
            repository.Delete(item);
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool> where = null)
        {
            return repository.GetAll(where);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task InsertAsync(TEntity item)
        {
            await repository.CreateAsync(item);
        }

        public void Update(TEntity item)
        {
            repository.Update(item);
        }

        public async Task SaveAsync()
        {
            await repository.SaveAsync();
        }
    }
}
