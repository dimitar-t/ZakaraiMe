namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface of the BaseRepository class
    /// </summary>
    /// <typeparam name="TEntity">Entity which inherits IBaseEntity</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        /// <summary>
        /// Gets all entities from the database which match the filter.
        /// </summary>
        /// <param name="filter">Function used for filtering only the needed data</param>
        /// <returns>Collection of <typeparamref name="TEntity"/></returns>
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> filter = null);

        /// <summary>
        /// Asynchronously gets an entry of <typeparamref name="TEntity"/>, if any, which has the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The entity or null</returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Asynchronously inserts an entry of <typeparamref name="TEntity"/> in the database
        /// </summary>
        /// <param name="entity">The new entry.</param>
        /// <returns></returns>
        Task CreateAsync(TEntity entity);

        /// <summary>
        /// Updates the entry of <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="entity">The entry to be updated.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes the entry of <typeparamref name="TEntity"/> from the database
        /// </summary>
        /// <param name="entity">The entry to be deleted.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Asynchronously invokes <code>dbSet.SaveChanges()</code>
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }
}
