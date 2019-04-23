namespace ZakaraiMe.Web.Models.Journeys
{
    using Common.Mapping;
    using Contracts;
    using Data.Entities.Implementations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class JourneySearchViewModel : IJourneyModel, IValidatableObject, IMapFrom<Journey>
    {
        public decimal StartPointX { get; set; }

        public decimal StartPointY { get; set; }

        public decimal EndPointX { get; set; }

        public decimal EndPointY { get; set; }

        public DateTime SetOffTime { get; set; }

        [Range(0, 10000)]
        public int Radius { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (SetOffTime < DateTime.UtcNow)
            {
                yield return new ValidationResult(WebConstants.PastDateError);
            }

            if (StartPointX == 0 || StartPointY == 0 || EndPointX == 0 || EndPointY == 0)
            {
                yield return new ValidationResult(WebConstants.WaypointsNotSelected);
            }
        }
    }
}
