namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Implementations;
    using System.Drawing;
    using System.Threading.Tasks;

    public interface IPictureRepository
    {
        Task InsertIntoDatabaseAsync(Picture pictureEntity);

        void InsertIntoFileSystem(Picture pictureEntity, Bitmap bmp);

        Task DeleteAsync(Picture image);

        Task<Picture> GetByName(string name);
    }
}
