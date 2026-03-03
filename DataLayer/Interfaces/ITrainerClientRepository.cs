using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    /// <summary>
    /// Repository interface for trainer-client relationship operations
    /// </summary>
    public interface ITrainerClientRepository : IBaseRepository<TrainerClientEntity>
    {
        /// <summary>
        /// Gets all clients assigned to a trainer
        /// </summary>
        /// <param name="trainerId">The trainer identifier</param>
        /// <returns>Collection of trainer-client relationships</returns>
        Task<IEnumerable<TrainerClientEntity>> GetAllByTrainerIdAsync(Guid trainerId);
    }
}
