namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILoverRepository
    {
        void AddLoverLog(LoverLog loverLog);
        Task AddLoverLogAsync(LoverLog loverLog);
        List<LoverLog> GetLoverLogsByLoverGuid(string loverGuid);
        Task<List<LoverLog>> GetLoverLogsByLoverGuidAsync(string loverGuid);
        List<LoverLog> GetLoverLogsByLoverCloudUserId(string loverCloudUserGuid);
        Task<List<LoverLog>> GetLoverLogsByLoverCloudUserIdAsync(string loverCloudUserGuid);
        Lover GetLoverByLoverCloudUserId(string loverCloudUserId);
        Task<Lover> GetLoverByLoverCloudUserIdAsync(string loverCloudUserId);
    }
}
