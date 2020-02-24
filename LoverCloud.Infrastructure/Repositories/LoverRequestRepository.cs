namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public async Task<PaginatedList<LoverRequest>> GetAsync(
            LoverResourceQueryParameters parameters)
        {
            string userId = parameters.UserId;
            if (string.IsNullOrEmpty(userId)) 
                throw new ArgumentException(
                    $"Parameters - {nameof(LoverResourceQueryParameters.UserId)} is required!", 
                    nameof(LoverResourceQueryParameters.UserId));

            IQueryable<LoverRequest> loverRequests = _dbContext.LoverRequests
                .Where(x => x.ReceiverId == userId || x.RequesterId == userId);
            IQueryable<LoverRequest> paginatedLoverRequests = loverRequests
                .Include(x => x.Receiver)
                .Include(x => x.Requester)
                .Skip(parameters.PageSize*(parameters.PageIndex-1))
                .Take(parameters.PageSize);

            return new PaginatedList<LoverRequest>(
                parameters.PageIndex, parameters.PageSize,
                await loverRequests.CountAsync(), 
                await paginatedLoverRequests.ToListAsync());
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
