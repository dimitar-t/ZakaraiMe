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
                                                                 ServiceConstants.ProfilePictureHeight); // Resizes the uploaded picture to a specific size
                await repo.InsertIntoDatabaseAsync(pictureEntity); // send the entity to the repo                
                repo.InsertIntoFileSystem(pictureEntity, resizedPicture); // send the bitmap to the repo

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task DeleteAsync(Picture item)
        {
            await repo.DeleteAsync(item);
        }        
    }
}
