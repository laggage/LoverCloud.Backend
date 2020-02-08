namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Threading.Tasks;

    public interface ILoverAnniversaryRepository : IRepository<LoverAnniversary>
    {
        Task<PaginatedList<LoverAnniversary>> GetAsync(string userId, LoverAnniversaryParameters parameters);
    }
}
