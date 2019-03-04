namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICarService : IBaseService<Car>
    {
        /// <summary>
        /// Asynchronously gets all makes from the repository.
        /// </summary>
        /// <returns></returns>
        Task<IList<Make>> GetMakesAsync();

        /// <summary>
        /// Asynchronously gets Models of a Make
        /// </summary>
        /// <param name="makeId">The Id of the Make</param>
        /// <returns></returns>
        Task<IList<Model>> GetModelsAsync(int makeId);
    }
}
