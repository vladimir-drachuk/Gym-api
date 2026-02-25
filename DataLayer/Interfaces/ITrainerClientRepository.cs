using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    public interface ITrainerClientRepository : IBaseRepository<TrainerClientEntity>
    {
        Task<IEnumerable<TrainerClientEntity>> GetAllByTrainerIdAsync(Guid trainerId);
    }
}
