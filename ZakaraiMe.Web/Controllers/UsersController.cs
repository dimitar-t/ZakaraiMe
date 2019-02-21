namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Common;
    using Data.Entities.Implementations;
    using Infrastructure.Extensions;
    using Infrastructure.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;
    using Service.Contracts;
    using Service.Helpers;
    using System.Drawing;
    using System.Threading.Tasks;

    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IPictureService pictureService;
        private readonly IMapper mapper;
        private const string IndexAction = nameof(HomeController.Index);
        private const string HomeControllerString = "Home";
        private const string UserString = "потребител";

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IPictureService pictureService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.pictureService = pictureService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = CommonConstants.AdministratorRole)]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = CommonConstants.AdministratorRole)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = CommonConstants.AdministratorRole)]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return View(model);
            }

            Picture profilePicture = new Picture(); // Creates instance of Picture entity
            Image profilePictureImage = PictureServiceHelpers.ConvertIFormFileToImage(model.ImageFile); // Converts the uploaded image to System.Drawing.Image
            bool imageInsertSuccess = await pictureService.InsertAsync(profilePicture, profilePictureImage); // inserts image into database and file system

            if (!imageInsertSuccess) // if something with the image goes wrong return error
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return View(model);
            }

            User user = new User
            {
                UserName = AuthenticationHelpers.GenerateUniqueUsername(model.FirstName, model.LastName),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                ProfilePictureFileName = profilePicture.FileName
            };

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) // If something with the insert of a user goes wrong return an error
            {
                await pictureService.DeleteAsync(profilePicture); // In that case delete the already inserted profile picture
                AddErrors(result);
                return View(model);
            }

            TempData.AddSuccessMessage(WebConstants.SuccessfulCreate, UserString);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            User userToEdit = await userManager.FindByIdAsync(id.ToString());

            if (userToEdit == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToHome();
            }

            User currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser.Id != id && !await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
            {                
                TempData.AddErrorMessage(WebConstants.Unauthorized, userToEdit.FirstName);
                return RedirectToHome();
            }

            UserFormViewModel userFormViewModel = mapper.Map<User, UserFormViewModel>(userToEdit);

            return View(userFormViewModel);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                TempData.AddErrorMessage(error.Description);
            }
        }

        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction(IndexAction, HomeControllerString, new { area = "" });
        }
    }
}
