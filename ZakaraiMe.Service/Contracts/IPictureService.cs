namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;
    using System.Drawing;
    using System.Threading.Tasks;

    public interface IPictureService
    {
        /// <summary>
        /// Inserts and resizes picture
        /// </summary>
        /// <param name="image">The picture which is to be inserted</param>
        /// <param name="profilePicture">The picture converted to System.Drawing.Image</param>
        /// <returns></returns>
        Task<bool> InsertAsync(Picture image, Image profilePicture);

        /// <summary>
        /// Deletes the picture
        /// </summary>
        /// <param name="item">The picture which is to be </param>
        /// <returns></returns>
        Task DeleteAsync(Picture item);
    }
}
