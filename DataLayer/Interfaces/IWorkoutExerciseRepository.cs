using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    /// <summary>
    /// Repository interface for workout exercise operations
    /// </summary>
    public interface IWorkoutExerciseRepository : IBaseRepository<WorkoutExerciseEntity>
    {
        /// <summary>
        /// Gets the user identifier associated with a workout exercise
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        /// <returns>The user identifier if found, otherwise null</returns>
        Task<Guid?> GetUserIdByWorkoutExerciseIdAsync(Guid workoutExerciseId);
        
        /// <summary>
        /// Gets the next order number for exercises in a workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>The next order number</returns>
        Task<int> GetNextOrderAsync(Guid workoutId);
        
        /// <summary>
        /// Recalculates the order of exercises after deletion
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        Task RecalculateOrderAfterDeleteAsync(Guid workoutId);
        
        /// <summary>
        /// Reorders an exercise within a workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <param name="exerciseId">The exercise identifier</param>
        /// <param name="currentOrder">The current order position</param>
        /// <param name="newOrder">The new order position</param>
        Task ReorderExerciseAsync(Guid workoutId, Guid exerciseId, int currentOrder, int newOrder);
        
        /// <summary>
        /// Checks if a workout has any actual (executed) exercises
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>True if any actual exercises exist, otherwise false</returns>
        Task<bool> AnyActualAsync(Guid workoutId);
        
        /// <summary>
        /// Finds an actual exercise by its planned exercise identifier
        /// </summary>
        /// <param name="plannedWorkoutExerciseId">The planned workout exercise identifier</param>
        /// <returns>The actual exercise if found, otherwise null</returns>
        Task<WorkoutExerciseEntity?> FindActualByPlannedIdAsync(Guid plannedWorkoutExerciseId);
        
        /// <summary>
        /// Deletes all planned exercises for a workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        Task DeletePlannedByWorkoutIdAsync(Guid workoutId);
        
        /// <summary>
        /// Deletes all actual exercises for a workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        Task DeleteActualByWorkoutIdAsync(Guid workoutId);
        
        /// <summary>
        /// Gets all exercises for a workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>Collection of workout exercises</returns>
        Task<IEnumerable<WorkoutExerciseEntity>> GetByWorkoutIdAsync(Guid workoutId);
        
        /// <summary>
        /// Gets all exercises for a workout with details
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>Collection of workout exercises with details</returns>
        Task<IEnumerable<WorkoutExerciseEntity>> GetByWorkoutIdWithDetailsAsync(Guid workoutId);
    }
}
