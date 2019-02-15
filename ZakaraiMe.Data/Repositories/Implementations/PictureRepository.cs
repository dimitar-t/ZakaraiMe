namespace ZakaraiMe.Data.Repositories.Implementations
{
    using Contracts;
    using Entities.Implementations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;

    public class PictureRepository : IPictureRepository
    {
        private readonly string wwwRootPath;
        private const string ImagesFolder = @"\images\database\";
        private const string FormatConstant = ".jpeg";
        private ZakaraiMeContext context;
        private DbSet<Picture> dbSet;

        public PictureRepository(ZakaraiMeContext context, IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            dbSet = context.Set<Picture>();
            wwwRootPath = hostingEnvironment.WebRootPath;
        }

        public async Task InsertAsync(Picture image, /*IFormFile formFile,*/ Bitmap bmp)
        {
            await dbSet.AddAsync(image); // Adds the image to the database
            context.SaveChanges();

            string imagePath = GetPictureFilePath(image.FileName); // Gets the picture path

            bmp.Save(imagePath, ImageFormat.Jpeg);
        }

        public void Delete(Picture image)
        {
            dbSet.Remove(image);
            context.Entry(image).State = EntityState.Deleted;
            context.SaveChanges(); // Deletes the image from the database

            string imagePath = GetPictureFilePath(image.FileName); // Gets the picture path

            File.Delete(imagePath); // Deletes the image from the file system
        }

        private string GetPictureFilePath(string imageName)
        {
            return wwwRootPath + ImagesFolder + imageName + FormatConstant;
        }
    }
}
