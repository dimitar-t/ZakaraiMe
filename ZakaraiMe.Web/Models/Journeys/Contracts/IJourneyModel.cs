namespace ZakaraiMe.Web.Models.Journeys.Contracts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public interface IJourneyModel
    {
        decimal StartPointX { get; set; }

        decimal StartPointY { get; set; }

        decimal EndPointX { get; set; }

        decimal EndPointY { get; set; }

        IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
