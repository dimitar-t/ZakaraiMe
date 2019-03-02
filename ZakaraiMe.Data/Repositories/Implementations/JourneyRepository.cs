namespace ZakaraiMe.Data.Repositories.Implementations
{
    using Contracts;
    using Entities.Implementations;

    public class JourneyRepository : BaseRepository<Journey>, IJourneyRepository
    {
        public JourneyRepository(ZakaraiMeContext context) : base(context)
        {
        }
    }
}
