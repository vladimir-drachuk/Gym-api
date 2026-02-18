using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    public interface IWorkoutRepository : IBaseRepository<Workout>
    {
        public Task<IEnumerable<Workout>> GetAllByUserIdAsync(Guid userId);
        public Task<IEnumerable<Workout>> GetAllByWorkoutPlanIdAsync(Guid workoutPlanId);
        
    }
}
