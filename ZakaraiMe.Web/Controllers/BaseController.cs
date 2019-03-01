namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Data.Entities.Contracts;
    using Data.Entities.Implementations;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class BaseController<TEntity, TFormViewModel, TListViewModel> : Controller where TEntity : class, IBaseEntity
                                                                                               where TFormViewModel : FormViewModel
                                                                                               where TListViewModel : ListViewModel

    {
        protected readonly IBaseService<TEntity> service;
        private readonly UserManager<User> userManager;
        protected readonly IMapper mapper;
        private const string IndexAction = nameof(HomeController.Index);
        private const string HomeControllerString = "Home";

        public BaseController(IBaseService<TEntity> service, UserManager<User> userManager, IMapper mapper)
        {
            this.service = service;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        protected abstract string ItemName { get; set; }

        protected abstract TFormViewModel SendFormData(TEntity item, TFormViewModel viewModel);

        protected abstract Task<TEntity> GetEntityAsync(TFormViewModel viewModel, int id);

        protected async Task<User> GetCurrentUserAsync()
            => await userManager.GetUserAsync(User);

        protected RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction(IndexAction, HomeControllerString, new { area = "" });
        }

        [Authorize]
        public virtual async Task<IActionResult> Index()
        {
            IEnumerable<TListViewModel> items = mapper.Map<IEnumerable<TListViewModel>>(await service.GetFilteredItemsAsync(await GetCurrentUserAsync()));            

            return View(items);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            TFormViewModel viewModel = SendFormData(null, null);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public virtual async Task<IActionResult> Create(TFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);

                SendFormData(null, viewModel);
                return View(viewModel);
            }

            TEntity item = await GetEntityAsync(viewModel, 0);

            if (item == null) // The viewmodel didn't pass a validation
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);

                SendFormData(null, viewModel);
                return View(viewModel);
            }

            if (service.IsItemDuplicate(item))
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);

                SendFormData(null, viewModel);
                return View(viewModel);
            }

            await service.InsertAsync(item);
            await service.SaveAsync();

            TempData.AddSuccessMessage(WebConstants.SuccessfulCreate, ItemName);

            return RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public virtual async Task<IActionResult> Update(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToHome();
            }

            if (!await service.IsUserAuthorizedAsync(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage(WebConstants.Unauthorized, ItemName);
                return RedirectToHome();
            }

            TFormViewModel viewModel = mapper.Map<TEntity, TFormViewModel>(item);

            SendFormData(item, viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public virtual async Task<IActionResult> Update(TFormViewModel viewModel, int id)
        {
            TEntity item = await GetEntityAsync(viewModel, id);

            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);

                SendFormData(item, viewModel);
                return View(viewModel);
            }

            if (!await service.IsUserAuthorizedAsync(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage(WebConstants.Unauthorized, ItemName);
                return RedirectToHome();
            }

            if (service.IsItemDuplicate(item))
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);

                SendFormData(item, viewModel);
                return View(viewModel);
            }

            service.Update(item);
            await service.SaveAsync();

            TempData.AddSuccessMessage(WebConstants.SuccessfulUpdate, ItemName);
            return RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public virtual async Task<IActionResult> Delete(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return NotFound();
            }

            if (!await service.IsUserAuthorizedAsync(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage(WebConstants.Unauthorized, ItemName);
                return Unauthorized();
            }

            service.Delete(item);

            return RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToHome();
            }

            TListViewModel viewModel = mapper.Map<TEntity, TListViewModel>(item);

            return View(viewModel);
        }
    }
}