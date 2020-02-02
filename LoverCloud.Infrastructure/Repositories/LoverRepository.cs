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

    public class LoverRepository : ILoverRepository
    {
        private readonly LoverCloudDbContext dbContext;

        public LoverRepository(LoverCloudDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddLoverLog(LoverLog loverLog)
        {
            dbContext.LoverLogs.Add(loverLog);
        }

        public Task AddLoverLogAsync(LoverLog loverLog)
        {
            return Task.Run(() => AddLoverLog(loverLog));
        }

        public List<LoverLog> GetLoverLogsByLoverGuid(string loverGuid)
        {
            if (string.IsNullOrEmpty(loverGuid)) throw new ArgumentNullException(nameof(loverGuid));
            return dbContext.LoverLogs.Where(x => x.LoverGuid == loverGuid).ToList();
        }

        public Task<List<LoverLog>> GetLoverLogsByLoverGuidAsync(string loverGuid)
        {
            return Task.Run(() => GetLoverLogsByLoverGuid(loverGuid));
        }

        public List<LoverLog> GetLoverLogsByLoverCloudUserId(string loverCloudUserGuid)
        {
            if (string.IsNullOrEmpty(loverCloudUserGuid)) throw new ArgumentNullException(nameof(loverCloudUserGuid));
            string loverGuid = dbContext.LoverCloudUsers.Include(x => x.Lover)
                .FirstOrDefault(x => x.Id == loverCloudUserGuid).Lover?.Guid;
            if (string.IsNullOrEmpty(loverGuid)) throw new Exception($"用户Id为{loverCloudUserGuid}的用户, 还没有对应的情侣");
            return GetLoverLogsByLoverGuid(loverGuid);
        }

        public Task<List<LoverLog>> GetLoverLogsByLoverCloudUserIdAsync(string loverCloudUserGuid)
        {
            return Task.Run(() => GetLoverLogsByLoverCloudUserId(loverCloudUserGuid));
        }

        public Lover GetLoverByLoverCloudUserId(string loverCloudUserId)
        {
            return dbContext.LoverCloudUsers.Include(x => x.Lover)
                .FirstOrDefault(x => x.Id == loverCloudUserId).Lover;
        }

        public Task<Lover> GetLoverByLoverCloudUserIdAsync(string loverCloudUserId)
        {
            return Task.Run(() => GetLoverByLoverCloudUserId(loverCloudUserId));
        }

        public void AddLoverRequest(LoverRequest loverRequest)
        {
            dbContext.LoverRequests.Add(loverRequest);
        }

        public Task AddLoverRequestAsync(LoverRequest loverRequest)
        {
            return Task.Run(() => AddLoverRequest(loverRequest));
        }

        public LoverRequest GetLoverRequestByGuid(string guid)
        {
            return dbContext.LoverRequests
                .Include(x => x.Receiver)
                .Include(x => x.Requester)
                .FirstOrDefault(x => x.Guid == guid);
        }

        public Task<LoverRequest> GetLoverRequestByGuidAsync(string guid)
        {
            return Task.Run(() => GetLoverRequestByGuid(guid));
        }

        public async Task AddLoverAsync(Lover lover)
        {
            await dbContext.Lovers.AddAsync(lover);
        }
    }
}
