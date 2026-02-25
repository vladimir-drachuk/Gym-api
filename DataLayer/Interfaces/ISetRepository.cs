using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    public interface ISetRepository : IBaseRepository<SetEntity>
    {
        Task<int> GetNextOrderAsync(Guid workoutExerciseId);
        Task RecalculateOrderAfterDeleteAsync(Guid workoutExerciseId);
    }
}
