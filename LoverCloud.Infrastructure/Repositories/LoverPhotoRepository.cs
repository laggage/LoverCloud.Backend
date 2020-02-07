namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LoverPhotoRepository:ILoverPhotoRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public LoverPhotoRepository(LoverCloudDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<LoverPhoto>> GetLoverPhotosAsync(string userId, LoverPhotoParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"Argument {userId} with type string can not be null or empty");

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

        public void Add(LoverPhoto entity)
        {
            _dbContext.LoverPhotos.Add(entity);
        }

        public void Delete(LoverPhoto entity)
        {
            _dbContext.LoverPhotos.Remove(entity);
        }

        public void Update(LoverPhoto entity)
        {
            _dbContext.Update(entity);
        }

        public Task<LoverPhoto> FindByIdAsync(string id)
        {
            return _dbContext.LoverPhotos.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
