namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoverAnniversaryRepository : ILoverAnniversaryRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public LoverAnniversaryRepository(LoverCloudDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public void Add(LoverAnniversary entity)
        {
            _dbContext.LoverAnniversaries.Add(entity);
        }

        public void Delete(LoverAnniversary entity)
        {
            _dbContext.LoverAnniversaries.Remove(entity);
        }

        public void Update(LoverAnniversary entity)
        {
            _dbContext.LoverAnniversaries.Update(entity);
        }

        public Task<LoverAnniversary> FindByIdAsync(string id)
        {
            return _dbContext.LoverAnniversaries
                .Include(x => x.Lover).ThenInclude(x => x.LoverCloudUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<LoverAnniversary>> GetAsync(string userId, LoverAnniversaryParameters parameters)
        {
            IQueryable<LoverAnniversary> paginatedQuery = _dbContext.LoverAnniversaries
                .Where(x => x.Lover.LoverCloudUsers.Any(user => user.Id == userId))
                .Skip(parameters.PageSize * (parameters.PageIndex - 1))
                .Take(parameters.PageSize);

            return new PaginatedList<LoverAnniversary>(
                parameters.PageIndex, parameters.PageSize,
                await paginatedQuery.CountAsync(), await paginatedQuery.ToListAsync());
        }
    }
}
