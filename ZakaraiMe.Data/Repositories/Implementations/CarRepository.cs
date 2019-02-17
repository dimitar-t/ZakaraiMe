
namespace ZakaraiMe.Data.Repositories.Implementations
{
    using ZakaraiMe.Data.Entities.Implementations;
    using ZakaraiMe.Data.Repositories.Contracts;

    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        public CarRepository(ZakaraiMeContext context) : base(context)
        {
        }
    }
}
