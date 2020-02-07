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

        public Task<List<LoverLog>> GetByLoverIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            return _dbContext.LoverLogs.Where(x => x.LoverId == userId).ToListAsync();
        }

        public async Task<List<LoverLog>> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            string loverId = _dbContext.Users.Include(x => x.Lover)
                .FirstOrDefault(x => x.Id == userId).Lover?.Id;
            if (string.IsNullOrEmpty(loverId)) throw new Exception($"用户Id为{userId}的用户, 还没有对应的情侣");
            return await GetByLoverIdAsync(loverId);
        }

        public Task<LoverLog> FindByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
