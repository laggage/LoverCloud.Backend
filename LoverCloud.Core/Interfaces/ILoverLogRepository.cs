namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Threading.Tasks;

    public interface ILoverLogRepository : IRepository<LoverLog>
    {
        Task<PaginatedList<LoverLog>> GetLoverLogsAsync(string userId, LoverLogParameters parameters);
    }
}
