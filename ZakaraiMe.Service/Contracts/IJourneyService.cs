namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;

    public interface IJourneyService : IBaseService<Journey>
    {
        /// <summary>
        /// Makes a user join a journey
        /// </summary>
        /// <param name="journey">Entry of the journey</param>
        /// <param name="currentUserId">Id of the user</param>
        void JoinJourney(Journey journey, int currentUserId);

        /// <summary>
        /// Makes a user leave a journey
        /// </summary>
        /// <param name="journey">Entry of the journey</param>
        /// <param name="currentUserId">Id of the user</param>
        void LeaveJourney(Journey journey, int currentUserId);
    }
}
