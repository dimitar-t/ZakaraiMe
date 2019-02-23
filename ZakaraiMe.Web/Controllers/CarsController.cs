namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using ZakaraiMe.Data.Entities.Implementations;
    using ZakaraiMe.Service.Contracts;
    using ZakaraiMe.Web.Models.Cars;

    public class CarsController : BaseController<Car, CarFormViewModel, CarListViewModel>
    {
        public CarsController(ICarService carService, UserManager<User> userManager, IMapper mapper) : base(carService, userManager, mapper)
        { 
        }

        protected override string ItemName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        protected override Task<Car> GetEntityAsync(CarFormViewModel viewModel, int id)
        {
            throw new System.NotImplementedException();
        }

        protected override CarFormViewModel SendFormData(Car item, CarFormViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
