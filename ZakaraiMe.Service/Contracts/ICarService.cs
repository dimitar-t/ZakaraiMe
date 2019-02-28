namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    public interface ICarService : IBaseService<Car>
    {
        Task<IList<Model>> GetModelsAsync();
    }
}
