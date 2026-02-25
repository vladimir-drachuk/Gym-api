using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    public interface IWorkoutExerciseRepository : IBaseRepository<WorkoutExerciseEntity>
    {
        Task<Guid?> GetUserIdByWorkoutExerciseIdAsync(Guid workoutExerciseId);
        Task<int> GetNextOrderAsync(Guid workoutId);
        Task RecalculateOrderAfterDeleteAsync(Guid workoutId);
        Task ReorderExerciseAsync(Guid workoutId, Guid exerciseId, int currentOrder, int newOrder);
    }
}
