namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Threading.Tasks;

    public interface ILoverAlbumRepository : IRepository<LoverAlbum>
    {
        Task<PaginatedList<LoverAlbum>> GetLoverAlbumsAsync(string userId, LoverAlbumParameters parameters);
        Task<int> CountAsync(string loverId);
        Task<LoverPhoto> GetCoverImage(string albumId);
        Task<int> GetPhotosCount(string albumId);
    }
}
