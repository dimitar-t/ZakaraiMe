namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Implementations;
    using Microsoft.AspNetCore.Http;
    using System.Drawing;
    using System.Threading.Tasks;

    public interface IPictureRepository
    {
        Task InsertAsync(Picture image, /*IFormFile formFile,*/ Bitmap bmp);

        void Delete(Picture image);
    }
}
