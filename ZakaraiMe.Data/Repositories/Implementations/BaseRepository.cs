namespace ZakaraiMe.Data.Repositories.Implementations
{
    using Contracts;
    using Entities.Contracts;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly ZakaraiMeContext context;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(ZakaraiMeContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Gets all entries from the database which match the filter.
        /// </summary>
        /// <param name="filter">Function used for getting only the needed data. It is the same as in .Where method in System.Linq</param>
        /// <returns>Collection of entities</returns>
        /// <example>Usage: <code>IEnumerable<Car> cars = repository.GetAll(c => c.Id > 10)</code></example>
        public virtual IEnumerable<TEntity> GetAll(Func<TEntity, bool> filter = null)
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

        public async virtual Task<TEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async virtual Task CreateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
            context.Entry(entity).State = EntityState.Deleted;
        }

        public async virtual Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
