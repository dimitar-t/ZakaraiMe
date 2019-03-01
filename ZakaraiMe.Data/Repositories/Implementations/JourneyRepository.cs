namespace ZakaraiMe.Data.Repositories.Implementations
{
    using Entities.Implementations;

    public class JourneyRepository : BaseRepository<Journey>
    {
        public JourneyRepository(ZakaraiMeContext context) : base(context)
        {
        }
    }
}
