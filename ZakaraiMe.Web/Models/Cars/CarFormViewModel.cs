namespace ZakaraiMe.Web.Models.Cars
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using ZakaraiMe.Common.Mapping;
    using ZakaraiMe.Data.Entities.Implementations;

    public class CarFormViewModel : FormViewModel, IMapFrom<Car>, IHaveCustomMapping 
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = WebConstants.RequiredField)]
        [Display(Name = "Цвят")]
        public string Colour { get; set; }

        public int ModelId { get; set; }

        public int OwnerId { get; set; }

        [Display(Name = "Снимка на колата")]
        public IFormFile ImageFile { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<Car, CarFormViewModel>(); // TODO: тва е ненужно, щото IMapFrom-а точно това прави. тоя метод ConfigureMapping
                                                        // е за някви случай когато нещо тряя се ignore-не примерно.
        }
    }
}
