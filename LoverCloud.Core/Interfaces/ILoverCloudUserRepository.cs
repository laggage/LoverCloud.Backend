namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ILoverCloudUserRepository
    {
        Task<LoverCloudUser> FindByIdAsync(string id);
        Task<LoverCloudUser> FindByNameAsync(string userName);
        Task<LoverCloudUser> FindByIdAsync(
            string id,
            Func<IQueryable<LoverCloudUser>, IQueryable<LoverCloudUser>> configIncludeable = null);
        Task<PaginatedList<LoverCloudUser>> GetAsync(
            LoverCloudUserParameters parameters, 
            Func<IQueryable<LoverCloudUser>, IQueryable<LoverCloudUser>> configIncludeable = null);
    }
}
