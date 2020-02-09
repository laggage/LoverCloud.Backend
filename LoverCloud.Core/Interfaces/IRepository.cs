namespace LoverCloud.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface IRepository<T, TKey> where T: class, IEntity
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<T> FindByIdAsync(TKey id);
        //Task<T> FindByIdAsync(
        //   string id, Func<DbSet<T>, IQueryable<T>> configInclude);
    }

    public interface IRepository<T> : IRepository<T, string> where T : class, IEntity
    {
    }
}
