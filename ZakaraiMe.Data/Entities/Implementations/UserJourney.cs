namespace ZakaraiMe.Data.Entities.Implementations
{
    /// <summary>
    /// Entity which represents the join table between users and journeys.
    /// </summary>
    public class UserJourney
    {
        public virtual User User { get; set; }

        public int UserId { get; set; }

        public virtual Journey Journey { get; set; }

        public int JourneyId { get; set; }
    }
}
