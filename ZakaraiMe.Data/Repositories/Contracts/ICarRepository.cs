namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Implementations;
    using System.Threading.Tasks;

    public interface ICarRepository : IBaseRepository<Car>
    {
        void SeedMakesAndModels();

        Task<Model> GetModelAsync(int modelId);
    }
}
