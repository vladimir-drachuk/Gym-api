using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    public interface IWorkoutRepository : IBaseRepository<WorkoutEntity>
    {
        Task<IEnumerable<WorkoutEntity>> GetAllByUserIdAsync(Guid userId);
        Task<IEnumerable<WorkoutEntity>> GetAllByWorkoutPlanIdAsync(Guid workoutPlanId);
        Task<Guid?> GetUserIdByWorkoutId(Guid workoutId);
    }
}
