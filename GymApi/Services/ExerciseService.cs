using DataLayer.Interfaces;
using GymApi.Model;

namespace GymApi.Services
{
    public class ExerciseService(IExerciseRepository exerciseRepository)
    {
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;

        public async Task<List<Exercise>> GetAllExercises()
        {
            var entities = await _exerciseRepository.GetAllAsync();

            return [.. entities.Select(MapToExercise)];
        }

        public async Task<Exercise?> GetExerciseById(Guid id)
        {
            var entity = await _exerciseRepository.GetByIdAsync(id);

            return entity == null ? null : MapToExercise(entity);
        }

        public async Task<Exercise> CreateExercise(Exercise exercise)
        {
            var entity = new DataLayer.Entities.Exercise
            {
                Name = exercise.Name,
                Description = exercise.Description
            };
            var createdEntity = await _exerciseRepository.AddAsync(entity);

            return MapToExercise(createdEntity);
        }

        public async Task<Exercise> UpdateExercise(Exercise exercise)
        {
            var entity = new DataLayer.Entities.Exercise
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description
            };
            var updatedEntity = await _exerciseRepository.UpdateAsync(entity);

            return MapToExercise(updatedEntity);
        }

        public async Task RemoveExercise(Guid id)
        {
            await _exerciseRepository.DeleteAsync(id);
        }

        private static Exercise MapToExercise(DataLayer.Entities.Exercise e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
        };
    }
}
