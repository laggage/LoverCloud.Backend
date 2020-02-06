namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class LoverRequestRepository : ILoverRequestRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public LoverRequestRepository(LoverCloudDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(LoverRequest entity)
        {
            _dbContext.Add(entity);
        }

        public void Delete(LoverRequest entity)
        {
            _dbContext.Remove(entity);
        }

        public void Update(LoverRequest entity)
        {
            _dbContext.LoverRequests.Update(entity);
        }

        public Task<LoverRequest> FindByIdAsync(string id)
        {
            return _dbContext.LoverRequests
                .Include(x => x.Receiver).ThenInclude(x => x.Lover)
                .Include(x => x.Requester).ThenInclude(x => x.Lover)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
