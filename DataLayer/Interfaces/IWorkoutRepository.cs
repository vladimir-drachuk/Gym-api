using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    /// <summary>
    /// Repository interface for workout operations
    /// </summary>
    public interface IWorkoutRepository : IBaseRepository<WorkoutEntity>
    {
        /// <summary>
        /// Gets all workouts for a specific user
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns>Collection of workouts</returns>
        Task<IEnumerable<WorkoutEntity>> GetAllByUserIdAsync(Guid userId);
        
        /// <summary>
        /// Gets all workouts for a specific workout plan
        /// </summary>
        /// <param name="workoutPlanId">The workout plan identifier</param>
        /// <returns>Collection of workouts</returns>
        Task<IEnumerable<WorkoutEntity>> GetAllByWorkoutPlanIdAsync(Guid workoutPlanId);
        
        /// <summary>
        /// Gets the user identifier associated with a workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>The user identifier if found, otherwise null</returns>
        Task<Guid?> GetUserIdByWorkoutId(Guid workoutId);
        
        /// <summary>
        /// Gets a workout with its exercises and sets
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>The workout with exercises and sets if found, otherwise null</returns>
        Task<WorkoutEntity?> GetWithExercisesAndSetsAsync(Guid workoutId);
    }
}
