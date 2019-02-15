namespace ZakaraiMe.Service.Implementations
{
    using Contracts;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using Helpers;
    using Microsoft.AspNetCore.Http;
    using System.Drawing;
    using System.Threading.Tasks;

    public class PictureService : IPictureService
    {
        private readonly IPictureRepository repo;

        public PictureService(IPictureRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> InsertAsync(Picture pictureEntity, IFormFile formFile)
        {
            try
            {
                Bitmap resizedPicture = PictureHelpers.ResizePicture(formFile,
                                                                 ServiceConstants.ProfilePictureWidth,
                                                                 ServiceConstants.ProfilePictureHeight); // Resizes the uploaded p itureto a specific size
                await repo.InsertAsync(pictureEntity, resizedPicture); // send the bitmap and the entity to the repo

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Delete(Picture item)
        {
            repo.Delete(item);
        }        
    }
}
