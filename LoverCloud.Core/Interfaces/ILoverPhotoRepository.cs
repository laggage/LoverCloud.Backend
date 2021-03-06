﻿namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ILoverPhotoRepository : IRepository<LoverPhoto>
    {
        Task<PaginatedList<LoverPhoto>> GetLoverPhotosAsync(string userId, LoverPhotoParameters parameters);
        Task<LoverPhoto> FindByIdAsync(string id, Func<IQueryable<LoverPhoto>, IQueryable<LoverPhoto>> configIncludable = null);
    }
}
