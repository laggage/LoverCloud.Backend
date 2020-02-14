namespace LoverCloud.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoverLogRepository : ILoverLogRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public LoverLogRepository(LoverCloudDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(LoverLog entity)
        {
            _dbContext.Add(entity);
        }

        public void Delete(LoverLog entity)
        {
            _dbContext.Remove(entity);
        }

        public void Update(LoverLog entity)
        {
            _dbContext.Update(entity);
        }

        

        public async Task<PaginatedList<LoverLog>> GetLoverLogsAsync(string userId, LoverLogParameters parameters)
        {
            if (string.IsNullOrEmpty(userId)) 
                throw new ArgumentNullException(nameof(userId));

            IQueryable<LoverLog> query = _dbContext.LoverLogs.Include(x => x.LoverPhotos).Where(
                x => x.Lover.LoverCloudUsers.Any(user => user.Id == userId));
            IQueryable<LoverLog> result = query
                .Skip(parameters.PageSize*(parameters.PageIndex-1))
                .Take(parameters.PageSize);

            return new PaginatedList<LoverLog>(
                parameters.PageIndex, parameters.PageSize,
                await query.CountAsync(), await result.ToListAsync());
        }

        public Task<LoverLog> FindByIdAsync(string id)
        {
            return _dbContext.LoverLogs
                .Include(x => x.Lover).ThenInclude(x => x.LoverCloudUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<int> CountAsync(string loverId)
        {
            return _dbContext.LoverLogs.Where(x => x.LoverId == loverId).CountAsync();
        }
    }
}
