namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Threading.Tasks;

    public interface ILoverPhotoRepository : IRepository<LoverPhoto>
    {
        Task<PaginatedList<LoverPhoto>> GetLoverPhotosAsync(string userId, LoverPhotoParameters parameters);
    }
}
