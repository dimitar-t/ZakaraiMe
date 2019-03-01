namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using Data.Entities.Implementations;

    public interface ICarService : IBaseService<Car>
    {
        Task<IList<Model>> GetModelsAsync();
    }
}
