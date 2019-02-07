namespace ZakaraiMe.Web.Controllers
{
    using Data.Entities.Implementations;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;
    using System;
    using System.Threading.Tasks;
    using System.Linq;

    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private const string HomeControllerString = "Home";
        private const string IndexAction = nameof(HomeController.Index);

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                TempData.AddWarningMessage(WebConstants.AlreadyRegistered);
                return RedirectToHome();
            }

            if (!ModelState.IsValid)
            {
                TempData.AddWarningMessage(WebConstants.TryAgain);
                return View(model);
            }

            User user = new User { Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, UserName = model.FirstName }; // Extract a method for unique username in the not yet created users service.
            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            await signInManager.SignInAsync(user, false);

            TempData.AddSuccessMessage("You successfully registered!");
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
