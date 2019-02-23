namespace ZakaraiMe.Web.Models.Cars
{
    using AutoMapper;
    using ZakaraiMe.Common.Mapping;
    using ZakaraiMe.Data.Entities.Implementations;

    public class CarListViewModel : ListViewModel, IMapFrom<Car>, IHaveCustomMapping
    {
        public string Colour { get; set; }

        public virtual Model Model { get; set; }

        public int OwnerId { get; set; }

        public string PictureFileName { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<Car, CarListViewModel>(); // TODO: същото като в другия клас CarFormViewModel
        }
    }
}
