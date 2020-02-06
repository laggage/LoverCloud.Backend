namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Threading.Tasks;

    public interface ILoverRepository : IRepository<Lover>
    {
        Task<Lover> FindByUserIdAsync(string userId);
    }
}
