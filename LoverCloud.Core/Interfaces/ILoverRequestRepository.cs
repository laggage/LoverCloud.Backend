namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;
    using System.Threading.Tasks;

    public interface ILoverRequestRepository : IRepository<LoverRequest>
    {
        Task<PaginatedList<LoverRequest>> GetAsync(
            LoverResourceQueryParameters parameters);
    }
}
