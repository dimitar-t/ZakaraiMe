namespace ZakaraiMe.Service.Contracts
{
    using Data.Entities.Implementations;
    using System.Drawing;
    using System.Threading.Tasks;

    public interface IPictureService
    {
        Task<bool> InsertAsync(Picture image, Image profilePicture);

        Task DeleteAsync(Picture item);
    }
}
