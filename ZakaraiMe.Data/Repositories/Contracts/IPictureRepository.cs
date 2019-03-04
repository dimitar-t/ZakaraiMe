namespace ZakaraiMe.Data.Repositories.Contracts
{
    using Entities.Implementations;
    using System.Drawing;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface of the PictureRepository class
    /// </summary>
    public interface IPictureRepository
    {
        /// <summary>
        /// Asynchronously inserts the picture in the database.
        /// </summary>
        /// <param name="pictureEntity"></param>
        /// <returns></returns>
        Task InsertIntoDatabaseAsync(Picture pictureEntity);

        /// <summary>
        /// Inserts the picture in the file system
        /// </summary>
        /// <param name="pictureEntity">The entry of the picture which is to be inserted</param>
        /// <param name="bmp">Bitmap of the picture</param>
        void InsertIntoFileSystem(Picture pictureEntity, Bitmap bmp);

        /// <summary>
        /// Asynchronously deletes the picture from the database
        /// </summary>
        /// <param name="image">The entry which is to be deleted</param>
        /// <returns></returns>
        Task DeleteAsync(Picture image);

        /// <summary>
        /// Asynchronously gets the picture, if any, by its name
        /// </summary>
        /// <param name="name">The name of the desired picture</param>
        /// <returns></returns>
        Task<Picture> GetByName(string name);
    }
}
