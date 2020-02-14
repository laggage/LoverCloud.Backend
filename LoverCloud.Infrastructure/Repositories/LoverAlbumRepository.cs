namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoverAlbumRepository : ILoverAlbumRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public LoverAlbumRepository(LoverCloudDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task<PaginatedList<LoverAlbum>> GetLoverAlbumsAsync(string userId, LoverAlbumParameters parameters)
        {
            IQueryable<LoverAlbum> query = _dbContext.LoverAlbums
                .Include(x => x.Lover).ThenInclude(x => x.LoverCloudUsers)
                .Where(x => x.Lover.LoverCloudUsers.Any(user => user.Id == userId));

            IQueryable<LoverAlbum>  paginatedQuery = query.Skip((parameters.PageIndex - 1) * parameters.PageSize)
                .Take(parameters.PageSize);

            return new PaginatedList<LoverAlbum>(
                parameters.PageIndex, parameters.PageSize, 
                await query.CountAsync(), await paginatedQuery.ToListAsync());
        }

        public void Add(LoverAlbum entity)
        {
            _dbContext.LoverAlbums.Add(entity);
        }

        public void Delete(LoverAlbum entity)
        {
            _dbContext.LoverAlbums.Remove(entity);
        }

        public void Update(LoverAlbum entity)
        {
            _dbContext.LoverAlbums.Update(entity);
        }

        public Task<LoverAlbum> FindByIdAsync(string id)
        {
            return _dbContext.LoverAlbums
                .Include(x => x.Lover)
                .ThenInclude(x => x.LoverCloudUsers)
                .Include(x => x.Creater)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<int> CountAsync(string loverId)
        {
            return _dbContext.LoverAlbums.Where(x => x.LoverId == loverId).CountAsync();
        }
    }
}
