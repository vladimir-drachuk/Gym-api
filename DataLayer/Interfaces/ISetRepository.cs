using DataLayer.Entities;

namespace DataLayer.Interfaces
{
    /// <summary>
    /// Repository interface for set operations
    /// </summary>
    public interface ISetRepository : IBaseRepository<SetEntity>
    {
        /// <summary>
        /// Gets the next order number for sets in a workout exercise
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        /// <returns>The next order number</returns>
        Task<int> GetNextOrderAsync(Guid workoutExerciseId);
        
        /// <summary>
        /// Recalculates the order of sets after deletion
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        Task RecalculateOrderAfterDeleteAsync(Guid workoutExerciseId);
        
        /// <summary>
        /// Gets all sets for multiple workout exercises
        /// </summary>
        /// <param name="workoutExerciseIds">Collection of workout exercise identifiers</param>
        /// <returns>Collection of sets</returns>
        Task<IEnumerable<SetEntity>> GetByWorkoutExerciseIdsAsync(IEnumerable<Guid> workoutExerciseIds);
    }
}
