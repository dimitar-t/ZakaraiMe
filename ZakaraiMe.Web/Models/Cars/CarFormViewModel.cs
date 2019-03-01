namespace ZakaraiMe.Web.Models.Cars
{
    using Common.Mapping;
    using Data.Entities.Implementations;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class CarFormViewModel : FormViewModel, IMapFrom<Car>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = WebConstants.RequiredField)]
        [Display(Name = "Цвят")]
        public string Colour { get; set; }

        public int ModelId { get; set; }

        public int OwnerId { get; set; }

        [Display(Name = "Снимка на колата")]
        public IFormFile ImageFile { get; set; }
    }
}
