namespace ZakaraiMe.Data.Entities.Implementations
{
    public class UserJourney
    {
        public virtual User User { get; set; }

        public int UserId { get; set; }

        public virtual Journey Journey { get; set; }

        public int JourneyId { get; set; }
    }
}
