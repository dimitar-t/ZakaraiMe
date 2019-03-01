namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Common;
    using Data.Entities.Implementations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cars;
    using Service.Contracts;
    using Service.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;

    public class CarsController : BaseController<Car, CarFormViewModel, CarListViewModel>
    {
        private readonly UserManager<User> userManager;
        private readonly IPictureService pictureService;
        private readonly ICarService carService;

        public CarsController(ICarService carService, UserManager<User> userManager, IPictureService pictureService, IMapper mapper) : base(carService, userManager, mapper)
        {
            this.userManager = userManager;
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

                if (viewModel.ImageFile == null)
                {
                    return null;
                }

                Picture carPicture = new Picture(); // Creates instance of Picture entity
                Image carPictureImage = PictureServiceHelpers.ConvertIFormFileToImage(viewModel.ImageFile); // Converts the uploaded image to System.Drawing.Image

                if (carPictureImage == null) // The uploaded file is not a picture
                {
                    return null;
                }

                bool imageInsertSuccess = await pictureService.InsertAsync(carPicture, carPictureImage); // inserts image into database and file system

                if (!imageInsertSuccess)
                {
                    return null;
                }

                // Properties which should never be updated
                car.PictureFileName = carPicture.FileName;
                car.OwnerId = GetCurrentUserAsync().Result.Id;
            }

            // Properties which can be updated
            car.Colour = viewModel.Colour;
            car.ModelId = viewModel.ModelId;

            return car;
        }

        public override async Task<IActionResult> Create(CarFormViewModel viewModel)
        {
            IActionResult result = await base.Create(viewModel);

            if (result == base.RedirectToHome()) // If the create action was successful
            {
                User currentUser = await GetCurrentUserAsync();
                await userManager.AddToRoleAsync(currentUser, CommonConstants.DriverRole);
            }

            return result;
        }

        // Cars don't have an option for update
        public override async Task<IActionResult> Update(int id)
        {
            return NotFound();
        }

        // Cars don't have an option for update
        public override async Task<IActionResult> Update(CarFormViewModel viewModel, int id)
        {
            return NotFound();
        }

        protected override CarFormViewModel SendFormData(Car item, CarFormViewModel viewModel)
        {
            ViewData["Makes"] = carService.GetMakesAsync().Result;

            return null;
        }

        public async Task<JsonResult> GetModels(int makeId)
        {
            IList<CarModelListViewModel> models = mapper.Map<IList<Model>, IList<CarModelListViewModel>>(await carService.GetModelsAsync(makeId));

            return Json(models);
        }
    }
}
