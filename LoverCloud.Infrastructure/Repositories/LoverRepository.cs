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
        private readonly LoverCloudDbContext _dbContext;

        public LoverRepository(LoverCloudDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddLoverLog(LoverLog loverLog)
        {
            _dbContext.LoverLogs.Add(loverLog);
        }

        public Task AddLoverLogAsync(LoverLog loverLog)
        {
            return Task.Run(() => AddLoverLog(loverLog));
        }

        public List<LoverLog> GetLoverLogsByLoverGuid(string loverGuid)
        {
            if (string.IsNullOrEmpty(loverGuid)) throw new ArgumentNullException(nameof(loverGuid));
            return _dbContext.LoverLogs.Where(x => x.LoverGuid == loverGuid).ToList();
        }

        public Task<List<LoverLog>> GetLoverLogsByLoverGuidAsync(string loverGuid)
        {
            return Task.Run(() => GetLoverLogsByLoverGuid(loverGuid));
        }

        public List<LoverLog> GetLoverLogsByLoverCloudUserId(string loverCloudUserGuid)
        {
            if (string.IsNullOrEmpty(loverCloudUserGuid)) throw new ArgumentNullException(nameof(loverCloudUserGuid));
            string loverGuid = _dbContext.Users.Include(x => x.Lover)
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
            return _dbContext.Users.Include(x => x.Lover)
                .FirstOrDefault(x => x.Id == loverCloudUserId).Lover;
        }

        public Task<Lover> GetLoverByLoverCloudUserIdAsync(string loverCloudUserId)
        {
            return Task.Run(() => GetLoverByLoverCloudUserId(loverCloudUserId));
        }

        public void AddLoverRequest(LoverRequest loverRequest)
        {
            _dbContext.LoverRequests.Add(loverRequest);
        }

        public Task AddLoverRequestAsync(LoverRequest loverRequest)
        {
            return Task.Run(() => AddLoverRequest(loverRequest));
        }

        public LoverRequest GetLoverRequestByGuid(string guid)
        {
            return _dbContext.LoverRequests
                .Include(x => x.Receiver).ThenInclude(x => x.Lover)
                .Include(x => x.Requester).ThenInclude(x => x.Lover)
                .FirstOrDefault(x => x.Guid == guid);
        }

        public Task<LoverRequest> GetLoverRequestByGuidAsync(string guid)
        {
            return Task.Run(() => GetLoverRequestByGuid(guid));
        }

        public async Task AddLoverAsync(Lover lover)
        {
            await _dbContext.Lovers.AddAsync(lover);
        }

        public void AddLoverPhoto(LoverPhoto loverPhoto)
        {
            _dbContext.LoverPhotos.Add(loverPhoto);
        }

        public Task<LoverPhoto> FindLoverPhotoByGuid(string guid)
        {
            return _dbContext.LoverPhotos.FirstOrDefaultAsync(x => x.Guid == guid);
        }

        public async Task<PaginatedList<LoverPhoto>> GetLoverPhotosAsync(string userId, LoverPhotoParameters parameters)
        {
            if (string.IsNullOrEmpty(userId) ||
                parameters == null)
                throw new ArgumentNullException();

            var loverPhotos = _dbContext.LoverPhotos.Where(
                    x => x.Lover.LoverCloudUsers.Any(y => y.Id == userId));
            if (!string.IsNullOrEmpty(parameters.Name))
                loverPhotos = loverPhotos.Where(x => x.Name.Equals(parameters.Name));
            var loverPhotoList = await loverPhotos
                .Skip(parameters.PageSize * (parameters.PageIndex - 1))
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PaginatedList<LoverPhoto>(
                parameters.PageIndex, parameters.PageSize,
                await loverPhotos.CountAsync(), loverPhotoList);
        }
    }
}
