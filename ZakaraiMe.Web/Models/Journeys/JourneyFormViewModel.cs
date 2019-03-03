namespace ZakaraiMe.Web.Models.Journeys
{
    using AutoMapper;
    using Cars;
    using Contracts;
    using Common.Mapping;
    using Data.Entities.Implementations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class JourneyFormViewModel : FormViewModel, IJourneyModel, IValidatableObject, IMapFrom<Journey>, IHaveCustomMapping
    {
        public decimal StartPointX { get; set; }
        
        public decimal StartPointY { get; set; }

        public decimal EndPointX { get; set; }

        public decimal EndPointY { get; set; }

        [Display(Name = "Цена")]
        [Range(0, 100), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Места")]
        [Range(0, 6)]
        public int Seats { get; set; }

        public int CarId { get; set; }

        [Display(Name = "Дата и час")]
        public DateTime SetOffTime { get; set; }

        public IEnumerable<CarListViewModel> DriverCars { get; set; } = new List<CarListViewModel>();

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Journey, JourneyFormViewModel>()
                .ForMember(j => j.DriverCars, cfg => cfg.Ignore());
        }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (SetOffTime < DateTime.UtcNow)
            {
                yield return new ValidationResult(WebConstants.PastDateError);
            }

            if(StartPointX == 0 || StartPointY == 0 || EndPointX == 0 || EndPointY == 0)
            {
                yield return new ValidationResult(WebConstants.WaypointsNotSelected);
            }
        }
    }
}
