namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class MenstruationLogRepository:IMenstruationLogRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public MenstruationLogRepository(LoverCloudDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public void Add(MenstruationLog entity)
        {
            _dbContext.MenstruationLogs.Add(entity);
        }

        public void Delete(MenstruationLog entity)
        {
            foreach (MenstruationDescription description in entity.MenstruationDescriptions)
                _dbContext.MenstruationDescriptions.Remove(description);

            _dbContext.MenstruationLogs.Remove(entity);
        }

        public void Update(MenstruationLog entity)
        {
            _dbContext.MenstruationLogs.Update(entity);
        }

        public Task<MenstruationLog> FindByIdAsync(string id)
        {
            return _dbContext.MenstruationLogs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<MenstruationLog> FindByIdAsync(
            string id, Func<DbSet<MenstruationLog>,
                IQueryable<MenstruationLog>> configInclude = null)
        {
            IQueryable<MenstruationLog> logs = configInclude?.Invoke(_dbContext.MenstruationLogs) ?? 
                _dbContext.MenstruationLogs;
            return logs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<MenstruationLog>> GetAsync(string userId, MenstruationLogParameters parameters)
        {
            IQueryable<MenstruationLog> mlogs = _dbContext.MenstruationLogs.Where(x => x.LoverCloudUser.Id == userId);
            IQueryable<MenstruationLog> query = mlogs.Skip(parameters.PageSize*(parameters.PageIndex-1))
                .Take(parameters.PageSize);

            return new PaginatedList<MenstruationLog>(
                parameters.PageIndex, parameters.PageSize, await mlogs.CountAsync(), await query.ToListAsync());
        }
    }
}
