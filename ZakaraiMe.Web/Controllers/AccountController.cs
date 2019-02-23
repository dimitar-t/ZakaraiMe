namespace ZakaraiMe.Web.Controllers
{
    using Data.Entities.Implementations;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Helpers;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;
    using Service.Contracts;
    using Service.Helpers;
    using System.Drawing;
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IPictureService pictureService;
        private const string HomeControllerString = "Home";
        private const string AccountControllerString = "Account";
        private const string ExternalLoginViewString = "ExternalLogin";
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

            if (!ModelState.IsValid || model.ImageFile == null)
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
                TempData.AddErrorMessage(WebConstants.UserNotExist, model.Email);

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

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), AccountControllerString, new { returnUrl });
            AuthenticationProperties properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToAction(nameof(Login));
            }
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            // If the user does not have an account, then ask the user to create an account.
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = info.LoginProvider;

            string email = info.Principal.FindFirstValue(ClaimTypes.Email);                             // Extracts all the needed data
            string username = info.Principal.FindFirstValue(ClaimTypes.Name);                           // from person's facebook account
            string profilePictureUrl = info.Principal.FindFirstValue(CustomClaimTypes.Picture);

            byte[] profilePictureBytes = PictureWebHelpers.DownloadPicture(profilePictureUrl); // Downloads and converts the profile picture to bytes

            return View(ExternalLoginViewString, new ExternalLoginViewModel
            {
                Email = email,
                Username = username,
                ProfilePictureBytes = profilePictureBytes
            });
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                    return RedirectToHome();
                }

                Picture profilePicture = new Picture();

                Stream stream = new MemoryStream(model.ProfilePictureBytes);
                Image pictureImage = Image.FromStream(stream);
                bool imageInsertSuccess = await pictureService.InsertAsync(profilePicture, pictureImage); // inserts image into database and file system

                if (!imageInsertSuccess) // if something with the image goes wrong return error
                {
                    TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                    return View(nameof(ExternalLogin), model);
                }

                string[] personalNames = AuthenticationHelpers.GetNamesFromExternalLogin(model.Username); // Extracts the first and last name of the person
                User user = new User
                {
                    UserName = AuthenticationHelpers.GenerateUniqueUsername(personalNames[0], personalNames[1]),
                    Email = model.Email,
                    FirstName = personalNames[0],
                    LastName = personalNames[1],
                    PhoneNumber = model.PhoneNumber,
                    ProfilePictureFileName = profilePicture.FileName
                };

                IdentityResult result = await userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
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
