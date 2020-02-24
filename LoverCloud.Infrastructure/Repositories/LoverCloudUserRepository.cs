namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoverCloudUserRepository : ILoverCloudUserRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public LoverCloudUserRepository(LoverCloudDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<LoverCloudUser> FindByIdAsync(string id)
        {
            return _dbContext.Users
                .Include(x => x.LoverRequests)
                .Include(x => x.Lover)
                .ThenInclude(x => x.LoverCloudUsers)
                .Include(x => x.Lover)
                .ThenInclude(x => x.LoveDay)
                .Include(x => x.Lover)
                .ThenInclude(x => x.CoverImage)
                .Include(x => x.Lover)
                .ThenInclude(x => x.WeddingDay)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<LoverCloudUser> FindByIdAsync(
            string id, 
            Func<IQueryable<LoverCloudUser>, IQueryable<LoverCloudUser>> configIncludeable = null)
        {
            IQueryable<LoverCloudUser> users = configIncludeable?.Invoke(_dbContext.Users) ?? _dbContext.Users;
            return users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<LoverCloudUser> FindByNameAsync(string userName)
        {
            return _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<PaginatedList<LoverCloudUser>> GetAsync(
            LoverCloudUserParameters parameters, Func<IQueryable<LoverCloudUser>, IQueryable<LoverCloudUser>> configIncludeable = null)
        {
            IQueryable<LoverCloudUser> users = configIncludeable?.Invoke(
                _dbContext.Users) ?? _dbContext.Users;

            users = users.Where(
                u => string.IsNullOrEmpty(parameters.UserName)
                ? true
                : u.UserName.Contains(parameters.UserName));

            IQueryable<LoverCloudUser> paginatedUsers = users.Skip(parameters.PageSize*(parameters.PageIndex-1))
                .Take(parameters.PageSize);

            return new PaginatedList<LoverCloudUser>(
                parameters.PageIndex, 
                parameters.PageSize, 
                await users.CountAsync(), 
                await paginatedUsers.ToListAsync());
        }
    }
}
