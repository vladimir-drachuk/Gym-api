using DataLayer.Entities;
using System.Linq.Expressions;

namespace DataLayer.Interfaces
{
    /// <summary>
    /// Base repository interface for common CRUD operations
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Collection of all entities</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its identifier
        /// </summary>
        /// <param name="id">The entity identifier</param>
        /// <returns>The entity if found, otherwise null</returns>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets entities by their identifiers
        /// </summary>
        /// <param name="ids">Collection of entity identifiers</param>
        /// <returns>Collection of entities</returns>
        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Adds multiple entities
        /// </summary>
        /// <param name="entities">Collection of entities to add</param>
        /// <returns>Collection of added entities</returns>
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity by its identifier
        /// </summary>
        /// <param name="id">The entity identifier</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Checks if any entity matches the predicate
        /// </summary>
        /// <param name="predicate">The predicate to check</param>
        /// <returns>True if any entity matches, otherwise false</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
