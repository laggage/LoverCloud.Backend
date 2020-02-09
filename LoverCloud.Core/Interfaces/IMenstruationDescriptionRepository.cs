namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMenstruationDescriptionRepository : IRepository<MenstruationDescription>
    {
        Task<MenstruationDescription> FindByIdAsync(
           string id, Func<DbSet<MenstruationDescription>, IQueryable<MenstruationDescription>> configInclude);

        Task<PaginatedList<MenstruationDescription>> GetAsync(
            string userId, string menstruationLogId, MenstruationDescriptionParameters parameters,
            Func<DbSet<MenstruationDescription>, IQueryable<MenstruationDescription>> configInclude = null);
    }
}
