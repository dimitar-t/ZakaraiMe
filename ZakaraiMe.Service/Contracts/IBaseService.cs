namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Contracts;
    using Data.Entities.Implementations;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBaseService<TEntity> where TEntity : class, IBaseEntity
    {
        /// <summary>
        /// Asynchronously sends the entry to the repository
        /// </summary>
        /// <param name="item">The new entry.</param>
        /// <returns></returns>
        Task InsertAsync(TEntity item);

        /// <summary>
        /// Sends the entry to the repository update method
        /// </summary>
        /// <param name="item">The entry to be updated</param>
        void Update(TEntity item);

        /// <summary>
        /// Sends the entry to the repository delete method
        /// </summary>
        /// <param name="item">The entry to be deleted</param>
        void Delete(TEntity item);

        /// <summary>
        /// Gets the entry from the database
        /// </summary>
        /// <param name="id">Id of the entry</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entries from the database
        /// </summary>
        /// <param name="where">Function used for filtering only the needed data</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> where = null);

        /// <summary>
        /// Checks whether a user is authorized to modify <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="item">Entry which is to be modified</param>
        /// <param name="currentUser">The logged in user</param>
        /// <returns></returns>
        Task<bool> IsUserAuthorizedAsync(TEntity item, User currentUser);

        /// <summary>
        /// Gets all entries to which the user has access.
        /// </summary>
        /// <param name="currentUser">The logged in user</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetFilteredItemsAsync(User currentUser);

        /// <summary>
        /// Checks whether the navigation properties of an entry exist.
        /// </summary>
        /// <param name="item">Entry whose properties are checked</param>
        /// <param name="currentUser">The logged in user</param>
        /// <returns></returns>
        Task<bool> ForeignPropertiesExistAsync(TEntity item, User currentUser);

        /// <summary>
        /// Invokes repository.SaveAsync();
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();

        /// <summary>
        /// Checks whether the entry is unique.
        /// </summary>
        /// <param name="item">Entry which is checked</param>
        /// <returns></returns>
        bool IsItemDuplicate(TEntity item);
    }
}
