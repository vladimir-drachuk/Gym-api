using DataLayer.Entities;
using DataLayer.Interfaces;

namespace GymApi.Services
{
    /// <summary>
    /// Service for managing trainer-client relationships
    /// </summary>
    public class TrainerService(ITrainerClientRepository trainerClientRepository)
    {
        private readonly ITrainerClientRepository _trainerClientRepository = trainerClientRepository;

        /// <summary>
        /// Checks if an activity can be assigned from assigner to assignee
        /// </summary>
        /// <param name="assignerId">The identifier of the user assigning the activity</param>
        /// <param name="assigneeId">The identifier of the user receiving the activity</param>
        /// <returns>True if assignment is allowed, false otherwise</returns>
        public async Task<bool> CheckActivityAssign(Guid? assignerId, Guid? assigneeId)
        {
            if (!assignerId.HasValue || !assigneeId.HasValue)
            {
                return false;
            }

            if (assignerId.Equals(assigneeId))
            {
                return true;
            }

            return await _trainerClientRepository.AnyAsync(tc => tc.TrainerId == assignerId && tc.UserId == assigneeId);
        }

        /// <summary>
        /// Assigns a client to a trainer
        /// </summary>
        /// <param name="trainerId">The trainer identifier</param>
        /// <param name="userId">The client user identifier</param>
        /// <exception cref="InvalidOperationException">Thrown when user is already assigned to this trainer</exception>
        public async Task AssignClientAsync(Guid trainerId, Guid userId)
        {
            var alreadyAssigned = await _trainerClientRepository
                .AnyAsync(tc => tc.TrainerId == trainerId && tc.UserId == userId);

            if (alreadyAssigned)
            {
                throw new InvalidOperationException("User is already assigned to this trainer.");
            }

            var entity = new TrainerClientEntity
            {
                TrainerId = trainerId,
                UserId = userId,
                AssignedAt = DateTime.UtcNow,
            };

            await _trainerClientRepository.AddAsync(entity);
        }

        /// <summary>
        /// Unassigns a client from a trainer
        /// </summary>
        /// <param name="trainerId">The trainer identifier</param>
        /// <param name="userId">The client user identifier</param>
        /// <returns>True if the client was unassigned, false if not found</returns>
        public async Task<bool> UnassignClientAsync(Guid trainerId, Guid userId)
        {
            var entities = await _trainerClientRepository
                .GetAllByTrainerIdAsync(trainerId);

            var entity = entities.FirstOrDefault(tc => tc.UserId == userId);
            if (entity == null)
            {
                return false;
            }

            await _trainerClientRepository.DeleteAsync(entity.Id);
            return true;
        }

        /// <summary>
        /// Gets all clients assigned to a trainer
        /// </summary>
        /// <param name="trainerId">The trainer identifier</param>
        /// <returns>Collection of trainer-client relationships</returns>
        public async Task<IEnumerable<TrainerClientEntity>> GetClientsAsync(Guid trainerId)
        {
            return await _trainerClientRepository.GetAllByTrainerIdAsync(trainerId);
        }
    }
}
