namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ZakaraiMe.Data.Entities.Contracts;
    using ZakaraiMe.Data.Entities.Implementations;
    using ZakaraiMe.Service.Contracts;
    using ZakaraiMe.Web.Infrastructure.Extensions;
    using ZakaraiMe.Web.Models.View;

    public abstract class BaseController<TEntity, TViewModel> : Controller where TEntity : class, IBaseEntity where TViewModel : BaseViewModel
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

        protected abstract TViewModel SendFormData(TEntity item, TViewModel viewModel);

        protected abstract Task<TEntity> GetEntityAsync(TViewModel viewModel, int id);

        protected virtual async Task FillViewModelProps(IEnumerable<TViewModel> items)
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
            IEnumerable<TViewModel> items = mapper.Map<IEnumerable<TViewModel>>(await service.GetFilteredItemsAsync(await GetCurrentUserAsync()));

            await FillViewModelProps(items);

            return View(items);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            TViewModel viewModel = SendFormData(null, null);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage("Не сте попълнили полетата правилно!");

                SendFormData(null, viewModel);
                return View(viewModel);
            }

            TEntity item = await GetEntityAsync(viewModel, 0);

            if (service.IsItemDuplicate(item))
            {
                TempData.AddErrorMessage($"{ItemName} с подобни данни вече съществува!");

                SendFormData(null, viewModel);
                return View(viewModel);
            }

            await service.InsertAsync(item);
            TempData.AddSuccessMessage($"Успешно създадохте {ItemName}!");

            return RedirectToIndex();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage($"{ItemName} не беше открит!");
                return RedirectToIndex();
            }

            if (!service.IsUserAuthorized(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage($"Нямате право да променяте {ItemName}!");
                return RedirectToIndex();
            }

            TViewModel viewModel = mapper.Map<TEntity, TViewModel>(item);

            SendFormData(item, viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(TViewModel viewModel, int id)
        {
            TEntity item = await GetEntityAsync(viewModel, id);

            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage("Не сте попълнили полетата правилно!");

                SendFormData(item, viewModel);
                return View(viewModel);
            }

            if (!service.IsUserAuthorized(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage($"Нямате право да променяте {ItemName}!");
                return RedirectToIndex();
            }

            //if (service.IsItemDuplicate(item))
            //{
            //    TempData.AddErrorMessage($"{ItemName} with such data already exists!");

            //    SendFormData(item, viewModel);
            //    return View(viewModel);
            //}

            service.Update(item);

            TempData.AddSuccessMessage($"Успешно променихте {ItemName}!");
            return RedirectToIndex();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            TEntity item = await service.GetByIdAsync(id);

            if (item == null)
            {
                TempData.AddErrorMessage($"{ItemName} не беше открит!");
                return RedirectToIndex();
            }

            if (!service.IsUserAuthorized(item, await GetCurrentUserAsync()))
            {
                TempData.AddWarningMessage($"Нямате право да променяте {ItemName}!");
                return RedirectToIndex();
            }

            service.Delete(item);
            return RedirectToIndex();
        }
    }
}
