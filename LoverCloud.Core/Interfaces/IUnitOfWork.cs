using System.Threading.Tasks;

namespace LoverCloud.Core.Interfaces
{
    public interface IUnitOfWork
    {
        bool SaveChanges();
        Task<bool> SaveChangesAsync();
    }
}
