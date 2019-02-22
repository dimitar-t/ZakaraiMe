namespace ZakaraiMe.Data.Repositories.Contracts
{
    using ZakaraiMe.Data.Entities.Implementations;

    public interface ICarRepository : IBaseRepository<Car>
    {
        void SeedMakesAndModels();
    }
}
