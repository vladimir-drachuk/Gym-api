using DataLayer.Entities;
using DataLayer.Interfaces;
using GymApi.Model;

namespace GymApi.Services
{
    /// <summary>
    /// Service for managing exercises
    /// </summary>
    public class ExerciseService(IExerciseRepository exerciseRepository)
    {
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;

        /// <summary>
        /// Gets all exercises
        /// </summary>
        /// <returns>List of all exercises</returns>
        public async Task<List<Exercise>> GetAllExercises()
        {
            var entities = await _exerciseRepository.GetAllAsync();

            return [.. entities.Select(MapToExercise)];
        }

        /// <summary>
        /// Gets an exercise by its identifier
        /// </summary>
        /// <param name="id">The exercise identifier</param>
        /// <returns>The exercise if found, otherwise null</returns>
        public async Task<Exercise?> GetExerciseById(Guid id)
        {
            var entity = await _exerciseRepository.GetByIdAsync(id);

            return entity == null ? null : MapToExercise(entity);
        }

        /// <summary>
        /// Creates a new exercise
        /// </summary>
        /// <param name="exercise">The exercise data to create</param>
        /// <returns>The created exercise</returns>
        public async Task<Exercise> CreateExercise(Exercise exercise)
        {
            var entity = new ExerciseEntity
            {
                Name = exercise.Name,
                Description = exercise.Description,
            };
            var createdEntity = await _exerciseRepository.AddAsync(entity);

            return MapToExercise(createdEntity);
        }

        /// <summary>
        /// Updates an existing exercise
        /// </summary>
        /// <param name="exercise">The exercise data to update</param>
        /// <returns>The updated exercise</returns>
        public async Task<Exercise> UpdateExercise(Exercise exercise)
        {
            var entity = new ExerciseEntity
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
            };
            var updatedEntity = await _exerciseRepository.UpdateAsync(entity);

            return MapToExercise(updatedEntity);
        }

        /// <summary>
        /// Removes an exercise by its identifier
        /// </summary>
        /// <param name="id">The exercise identifier</param>
        public async Task RemoveExercise(Guid id)
        {
            await _exerciseRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Maps an exercise entity to an exercise model
        /// </summary>
        /// <param name="e">The exercise entity</param>
        /// <returns>The mapped exercise</returns>
        private static Exercise MapToExercise(ExerciseEntity e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
        };
    }
}
