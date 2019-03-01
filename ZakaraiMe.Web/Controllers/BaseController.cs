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
        private const string IndexAction = nameof(Index);

        public BaseController(IBaseService<TEntity> service, UserManager<User> userManager, IMapper mapper)
        {
            this.service = service;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        protected abstract string ItemName { get; set; }

        protected abstract TFormViewModel SendFormData(TEntity item, TFormViewModel viewModel);

        protected abstract Task<TEntity> GetEntityAsync(TFormViewModel viewModel, int id);

        protected virtual async Task FillViewModelProps(IEnumerable<TListViewModel> items)
        {
        }

        protected async Task<User> GetCurrentUserAsync()
            => await userManager.GetUserAsync(User);

        private RedirectToActionResult RedirectToIndex()
        {
            return RedirectToAction(IndexAction);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            IEnumerable<TListViewModel> items = mapper.Map<IEnumerable<TListViewModel>>(await service.GetFilteredItemsAsync(await GetCurrentUserAsync()));

            await FillViewModelProps(items);

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
        public async Task<IActionResult> Create(TFormViewModel viewModel)
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
            TempData.AddSuccessMessage(WebConstants.SuccessfulCreate, ItemName);

            return RedirectToIndex();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToIndex();
            }

            if (!await service.IsUserAuthorizedAsync(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage(WebConstants.Unauthorized, ItemName);
                return RedirectToIndex();
            }

            TFormViewModel viewModel = mapper.Map<TEntity, TFormViewModel>(item);

            SendFormData(item, viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(TFormViewModel viewModel, int id)
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
                return RedirectToIndex();
            }

            if (service.IsItemDuplicate(item))
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);

                SendFormData(item, viewModel);
                return View(viewModel);
            }

            service.Update(item);

            TempData.AddSuccessMessage(WebConstants.SuccessfulUpdate, ItemName);
            return RedirectToIndex();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToIndex();
            }

            if (!await service.IsUserAuthorizedAsync(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage(WebConstants.Unauthorized, ItemName);
                return RedirectToIndex();
            }

            service.Delete(item);
            return RedirectToIndex();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage(WebConstants.ErrorTryAgain);
                return RedirectToIndex();
            }

            TListViewModel viewModel = mapper.Map<TEntity, TListViewModel>(item);

            return View(viewModel);
        }
    }
}