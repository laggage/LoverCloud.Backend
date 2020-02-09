namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMenstruationLogRepository : IRepository<MenstruationLog>
    {
        Task<PaginatedList<MenstruationLog>> GetAsync(string userId, MenstruationLogParameters parameters);
        Task<MenstruationLog> FindByIdAsync(
            string id, Func<DbSet<MenstruationLog>,
                IQueryable<MenstruationLog>> configInclude = null);
    }
}
