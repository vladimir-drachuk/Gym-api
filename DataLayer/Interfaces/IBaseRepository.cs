using DataLayer.Entities;
using System.Linq.Expressions;

namespace DataLayer.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity?> GetByIdAsync(Guid id);

        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<Guid> ids);

        Task<TEntity> AddAsync(TEntity entity);

        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(Guid id);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
