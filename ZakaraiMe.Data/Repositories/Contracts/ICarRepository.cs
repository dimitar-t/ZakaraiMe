namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Implementations;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary cref="IBaseRepository{TEntity}">
    /// Interface of the CarRepository class with additional methods.
    /// </summary>
    public interface ICarRepository : IBaseRepository<Car>
    {
        /// <summary>
        /// Seeds static data of makes and models entries of cars from a query in the database.
        /// </summary>
        void SeedMakesAndModels();

        /// <summary>
        /// Asynchronously gets the Model which has the specified Id
        /// </summary>
        /// <param name="modelId">Id of the Model</param>
        /// <returns></returns>
        Task<Model> GetModelAsync(int modelId);

        /// <summary>
        /// Asynchronously gets all Makes
        /// </summary>
        /// <returns></returns>
        Task<IList<Make>> GetAllMakesAsync();

        /// <summary>
        /// Asynchronously gets all Models of the specified Make
        /// </summary>
        /// <param name="makeId">Id of the Model's Make</param>
        /// <returns></returns>
        Task<IList<Model>> GetAllModelsAsync(int makeId);
    }
}
