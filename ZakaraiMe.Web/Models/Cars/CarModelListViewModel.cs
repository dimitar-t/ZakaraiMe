namespace ZakaraiMe.Web.Models.Cars
{
    using Common.Mapping;
    using Data.Entities.Implementations;

    public class CarModelListViewModel : IMapFrom<Model>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
