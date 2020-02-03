namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
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
                .Include(x => x.Lover).ThenInclude(x => x.LoverCloudUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<LoverCloudUser> FindByNameAsync(string userName)
        {
            return _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }
    }
}
