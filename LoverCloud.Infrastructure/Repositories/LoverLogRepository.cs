using Microsoft.EntityFrameworkCore;

namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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

            IQueryable<LoverLog> paginatedQuery = _dbContext.LoverLogs.Where(
                x => x.Lover.LoverCloudUsers.Any(user => user.Id == userId))
                .Skip(parameters.PageSize*(parameters.PageIndex-1))
                .Take(parameters.PageSize);

            return new PaginatedList<LoverLog>(
                parameters.PageIndex, parameters.PageSize,
                await paginatedQuery.CountAsync(), await paginatedQuery.ToListAsync());
        }

        public Task<LoverLog> FindByIdAsync(string id)
        {
            return _dbContext.LoverLogs
                .Include(x => x.Lover).ThenInclude(x => x.LoverCloudUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
