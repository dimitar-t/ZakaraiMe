namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;
    using System;

    public interface IJourneyService : IBaseService<Journey>
    {
        void JoinJourney(Journey journey, int currentUserId);

        void LeaveJourney(Journey journey, int currentUserId);

        bool IsMatch(double startSearchLat, double startSearchLon, double endSearchLat, double endSearchLon, int radius, DateTime date, Journey journey);
    }
}
