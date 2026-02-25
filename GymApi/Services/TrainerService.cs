using DataLayer.Entities;
using DataLayer.Interfaces;

namespace GymApi.Services
{
    public class TrainerService(ITrainerClientRepository trainerClientRepository)
    {
        private readonly ITrainerClientRepository _trainerClientRepository = trainerClientRepository;

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

        public async Task<IEnumerable<TrainerClientEntity>> GetClientsAsync(Guid trainerId)
        {
            return await _trainerClientRepository.GetAllByTrainerIdAsync(trainerId);
        }
    }
}
