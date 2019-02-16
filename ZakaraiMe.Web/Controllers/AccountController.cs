namespace ZakaraiMe.Web.Controllers
{
    using Data.Entities.Implementations;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;
    using Service.Contracts;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IPictureService pictureService;
        private const string HomeControllerString = "Home";
        private const string IndexAction = nameof(HomeController.Index);

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IPictureService pictureService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.pictureService = pictureService;
        }

        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction(IndexAction, HomeControllerString);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddWarningMessage(WebConstants.AlreadyRegistered);

                return RedirectToHome();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserFormViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddErrorMessage(WebConstants.AlreadyRegistered);
                return RedirectToHome();
            }

            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return View(model);
            }

            Picture profilePicture = new Picture();
            bool imageInsertSuccess = await pictureService.InsertAsync(profilePicture, model.ImageFile); // inserts image into database and file system

            if (!imageInsertSuccess) // if something with the image goes wrong return error
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return View(model);
            }

            User user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.FirstName, // TODO: Extract a method for unique username in the not yet created users service.
                ProfilePictureFileName = profilePicture.FileName
            };            

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) // If something with the insert of a user goes wrong return an error
            {
                await pictureService.DeleteAsync(profilePicture); // In that case delete the already inserted profile picture
                AddErrors(result);
                return View(model);
            }

            await signInManager.SignInAsync(user, false); // Signs the new user

            TempData.AddSuccessMessage(WebConstants.SuccessfulRegistration);
            return RedirectToHome();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddWarningMessage(WebConstants.AlreadyLoggedIn);
                return RedirectToHome();
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);

                return View(model);
            }

            User userToLogin = await userManager.FindByEmailAsync(model.Email);

            if (userToLogin == null)
            {
                TempData.AddErrorMessage(string.Format(WebConstants.UserNotExist, model.Email));

                return View(model);
            }

            Microsoft.AspNetCore.Identity.SignInResult identityResult = await signInManager.PasswordSignInAsync(userToLogin.UserName, model.Password, model.RememberMe, false);

            if (!identityResult.Succeeded)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);

                return View(model);
            }

            TempData.AddSuccessMessage(WebConstants.WelcomeMessage);
            return RedirectToLocal(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            
            return RedirectToHome();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToHome();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                TempData.AddErrorMessage(error.Description);
            }
        }
    }
}
