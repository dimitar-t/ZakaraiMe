namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using System.Drawing;
    using System.Threading.Tasks;
    using ZakaraiMe.Data.Entities.Implementations;
    using ZakaraiMe.Service.Contracts;
    using ZakaraiMe.Service.Helpers;
    using ZakaraiMe.Web.Models.Cars;

    public class CarsController : BaseController<Car, CarFormViewModel, CarListViewModel>
    {
        private readonly IPictureService pictureService;
        private readonly ICarService carService;

        public CarsController(ICarService carService, UserManager<User> userManager, IPictureService pictureService, IMapper mapper) : base(carService, userManager, mapper)
        {
            this.pictureService = pictureService;
            this.carService = carService;
        }

        protected override string ItemName { get; set; } = "кола";

        protected override async Task<Car> GetEntityAsync(CarFormViewModel viewModel, int id)
        {
            Car car = await service.GetByIdAsync(id);

            if (car == null)
            {
                car = new Car();

                Picture carPicture = new Picture(); // Creates instance of Picture entity
                Image carPictureImage = PictureServiceHelpers.ConvertIFormFileToImage(viewModel.ImageFile); // Converts the uploaded image to System.Drawing.Image
                bool imageInsertSuccess = await pictureService.InsertAsync(carPicture, carPictureImage); // inserts image into database and file system

                if (imageInsertSuccess) 
                {
                    car.PictureFileName = carPicture.FileName;
                }
            }

            car.Colour = viewModel.Colour;
            car.ModelId = viewModel.ModelId;
            car.OwnerId = viewModel.OwnerId;

            return car;
        }

        protected override CarFormViewModel SendFormData(Car item, CarFormViewModel viewModel)
        {
            ViewData["Models"] = carService.GetModelsAsync().Result;

            return null;
        }
    }
}
