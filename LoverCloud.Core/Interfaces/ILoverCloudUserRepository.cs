namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Threading.Tasks;

    public interface ILoverCloudUserRepository
    {
        public Task<LoverCloudUser> FindByIdAsync(string id);
        public Task<LoverCloudUser> FindByNameAsync(string userName);
    }
}
