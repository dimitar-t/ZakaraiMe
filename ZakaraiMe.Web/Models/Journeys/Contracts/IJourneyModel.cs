namespace ZakaraiMe.Web.Models.Journeys.Contracts
{
    public interface IJourneyModel
    {
        decimal StartPointX { get; set; }

        decimal StartPointY { get; set; }

        decimal EndPointX { get; set; }

        decimal EndPointY { get; set; }
    }
}
