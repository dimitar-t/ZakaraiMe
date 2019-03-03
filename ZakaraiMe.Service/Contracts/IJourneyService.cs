namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;

    public interface IJourneyService : IBaseService<Journey>
    {
        void JoinJourney(Journey journey, int currentUserId);

        void LeaveJourney(Journey journey, int currentUserId);
    }
}
