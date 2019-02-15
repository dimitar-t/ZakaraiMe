namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IPictureService
    {
        Task<bool> InsertAsync(Picture image, IFormFile formFile);

        Task DeleteAsync(Picture item);
    }
}
