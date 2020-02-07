namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Threading.Tasks;

    public interface ILoverAlbumRepository : IRepository<LoverAlbum>
    {
        Task<PaginatedList<LoverAlbum>> GetLoverAlbumsAsync(string userId, LoverAlbumParameters parameters);
    }
}
