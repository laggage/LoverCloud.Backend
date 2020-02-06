namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILoverLogRepository : IRepository<LoverLog>
    {
        Task<List<LoverLog>> GetByUserIdAsync(string loverCloudUserId);
    }
}
