using DataLayer.Entities;
using DataLayer.Interfaces;
using GymApi.Model;

namespace GymApi.Services
{
    /// <summary>
    /// Service for managing workout plans
    /// </summary>
    public class WorkoutPlanService(IWorkoutPlanRepository workoutPlanRepository)
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;

        /// <summary>
        /// Gets all workout plans
        /// </summary>
        /// <returns>List of all workout plans</returns>
        public async Task<List<WorkoutPlan>> GetAll()
        {
            var entities = await _workoutPlanRepository.GetAllAsync();

            return [.. entities.Select(MapToWorkoutPlan)];
        }

        /// <summary>
        /// Gets a workout plan by its identifier
        /// </summary>
        /// <param name="id">The workout plan identifier</param>
        /// <returns>The workout plan if found, otherwise null</returns>
        public async Task<WorkoutPlan?> GetById(Guid id)
        {
            var entity = await _workoutPlanRepository.GetByIdAsync(id);

            return entity == null ? null : MapToWorkoutPlan(entity);
        }

        /// <summary>
        /// Creates a new workout plan
        /// </summary>
        /// <param name="createWorkoutPlan">The workout plan data to create</param>
        /// <returns>The created workout plan</returns>
        public async Task<WorkoutPlan> Create(CreateWorkoutPlan createWorkoutPlan)
        {
            var entity = new WorkoutPlanEntity
            {
                UserId = createWorkoutPlan.UserId,
                Description = createWorkoutPlan.Description,
            };

            var created = await _workoutPlanRepository.AddAsync(entity);

            return MapToWorkoutPlan(created);
        }

        /// <summary>
        /// Updates an existing workout plan
        /// </summary>
        /// <param name="id">The workout plan identifier</param>
        /// <param name="updateWorkoutPlan">The workout plan data to update</param>
        /// <returns>The updated workout plan if found, otherwise null</returns>
        public async Task<WorkoutPlan?> Update(Guid id, UpdateWorkoutPlan updateWorkoutPlan)
        {
            var entity = await _workoutPlanRepository.GetByIdAsync(id);
            if (entity == null)
                return null;

            entity.Description = updateWorkoutPlan.Description;

            var updated = await _workoutPlanRepository.UpdateAsync(entity);

            return MapToWorkoutPlan(updated);
        }

        /// <summary>
        /// Deletes a workout plan by its identifier
        /// </summary>
        /// <param name="id">The workout plan identifier</param>
        public async Task Delete(Guid id)
        {
            await _workoutPlanRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Maps a workout plan entity to a workout plan model
        /// </summary>
        /// <param name="w">The workout plan entity</param>
        /// <returns>The mapped workout plan</returns>
        private static WorkoutPlan MapToWorkoutPlan(WorkoutPlanEntity w) => new()
        {
            Id = w.Id,
            UserId = w.UserId,
            Description = w.Description,
            CreatedAt = w.CreatedAt,
        };
    }
}
