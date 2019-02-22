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
    using ZakaraiMe.Data.Helpers;

    public class PictureRepository : IPictureRepository
    {
        private readonly string wwwRootPath;        
        private ZakaraiMeContext context;
        private DbSet<Picture> dbSet;

        public PictureRepository(ZakaraiMeContext context, IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            dbSet = context.Set<Picture>();
            wwwRootPath = hostingEnvironment.WebRootPath;
        }

        public async Task InsertIntoDatabaseAsync(Picture pictureEntity)
        {
            await dbSet.AddAsync(pictureEntity); // Adds the image to the database
            await context.SaveChangesAsync();            
        }

        public void InsertIntoFileSystem(Picture pictureEntity, Bitmap bmp)
        {
            string imagePath = PictureDataHelpers.GetPictureFilePath(wwwRootPath, pictureEntity.FileName); // Gets the picture path

            bmp.Save(imagePath);
        }

        public async Task DeleteAsync(Picture pictureEntity)
        {
            dbSet.Remove(pictureEntity);
            context.Entry(pictureEntity).State = EntityState.Deleted;
            await context.SaveChangesAsync(); // Deletes the image from the database

            string imagePath = PictureDataHelpers.GetPictureFilePath(wwwRootPath, pictureEntity.FileName); // Gets the picture path

            File.Delete(imagePath); // Deletes the image from the file system
        }

        public async Task<Picture> GetByName(string name)
        {
            return await dbSet.FirstOrDefaultAsync(p => p.FileName == name);
        }
    }
}
