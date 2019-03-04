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
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
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
        public IActionResult Index()
        {
            IList<User> userEntities = userManager.Users.ToList();
            IList<UserListViewModel> usersViewModel = mapper.Map<List<UserListViewModel>>(userEntities);

            for (int i = 0; i < userEntities.Count(); i++)
            {
                IList<string> userRoles = userManager.GetRolesAsync(userEntities[i]).Result;
                usersViewModel[i].Roles = userRoles;
            }

            return View(usersViewModel);
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
            if (!ModelState.IsValid || model.ImageFile == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return View(model);
            }

            Picture profilePicture = new Picture(); // Creates instance of Picture entity
            Image profilePictureImage = PictureServiceHelpers.ConvertIFormFileToImage(model.ImageFile); // Converts the uploaded image to System.Drawing.Image

            if (profilePictureImage == null) // The uploaded file is not a picture
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return View(model);
            }

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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(UserFormViewModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return View(model);
            }

            User userToUpdate = await userManager.FindByIdAsync(id.ToString());
            if (userToUpdate == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToHome();
            }

            User currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser.Id != id && !await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
            {
                TempData.AddErrorMessage(WebConstants.Unauthorized, userToUpdate.FirstName);
                return RedirectToHome();
            }

            userToUpdate.Email = model.Email;
            userToUpdate.FirstName = model.FirstName;
            userToUpdate.LastName = model.LastName;
            userToUpdate.PhoneNumber = model.PhoneNumber;

            IdentityResult updateUserResult = await userManager.UpdateAsync(userToUpdate);
            if (!updateUserResult.Succeeded)
            {
                AddErrors(updateUserResult);
                return View(model);
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(userToUpdate);
            IdentityResult resetPasswordResult = await userManager.ResetPasswordAsync(userToUpdate, token, model.Password);
            if (!resetPasswordResult.Succeeded)
            {
                AddErrors(resetPasswordResult);
                return RedirectToHome();
            }

            if (currentUser.Id == id)
                await signInManager.SignInAsync(userToUpdate, false);


            TempData.AddSuccessMessage(WebConstants.SuccessfulUpdate);
            return RedirectToHome();
        }

        [HttpGet]
        [Authorize(Roles = CommonConstants.AdministratorRole)]
        public async Task<IActionResult> Delete(int id)
        {
            User userToDelete = await userManager.FindByIdAsync(id.ToString());
            if (userToDelete == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToAction(nameof(Index));
            }

            if (userToDelete.DriverJourneys.Count() != 0 || userToDelete.PassengerJourneys.Count() != 0)
            {
                TempData.AddErrorMessage(WebConstants.UserHasJourneys);
                return RedirectToHome();
            }

            Picture profilePictureToDelete = userToDelete.ProfilePicture;

            IdentityResult result = await userManager.DeleteAsync(userToDelete);

            if (!result.Succeeded)
            {
                AddErrors(result);
            }
            else
            {
                TempData.AddSuccessMessage(WebConstants.SuccessfulDelete, userToDelete.FirstName);
                await pictureService.DeleteAsync(profilePictureToDelete);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            User userToDisplay = await userManager.FindByIdAsync(id.ToString());
            if (userToDisplay == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToHome();
            }

            return View(userToDisplay);
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
