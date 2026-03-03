using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    /// <summary>
    /// Repository interface for user operations
    /// </summary>
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        /// <summary>
        /// Gets a user by email address
        /// </summary>
        /// <param name="email">The user email address</param>
        /// <returns>The user if found, otherwise null</returns>
        Task<UserEntity?> GetByEmailAsync(string email);
    }
}
