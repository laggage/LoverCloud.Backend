namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class LoverRepository : ILoverRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public LoverRepository(LoverCloudDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Lover> FindByIdAsync(string id)
        {
            return _dbContext.Lovers.FirstOrDefaultAsync(x => x.Id == id);
        }


        public void Add(Lover entity)
        {
            _dbContext.Lovers.Add(entity);
        }

        public void Delete(Lover entity)
        {
            _dbContext.Lovers.Remove(entity);
        }

        public void Update(Lover entity)
        {
            _dbContext.Lovers.Update(entity);
        }

        public async Task<Lover> FindByUserIdAsync(string userId)
        {
            return (await _dbContext.Users.Include(x => x.Lover)
                .FirstOrDefaultAsync(x => x.Id == userId)).Lover;
        }
    }
}
