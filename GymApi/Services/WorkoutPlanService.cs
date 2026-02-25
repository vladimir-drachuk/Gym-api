using DataLayer.Entities;
using DataLayer.Interfaces;
using GymApi.Model;

namespace GymApi.Services
{
    public class WorkoutPlanService(IWorkoutPlanRepository workoutPlanRepository)
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;

        public async Task<List<WorkoutPlan>> GetAll()
        {
            var entities = await _workoutPlanRepository.GetAllAsync();

            return [.. entities.Select(MapToWorkoutPlan)];
        }

        public async Task<WorkoutPlan?> GetById(Guid id)
        {
            var entity = await _workoutPlanRepository.GetByIdAsync(id);

            return entity == null ? null : MapToWorkoutPlan(entity);
        }

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

        public async Task<WorkoutPlan?> Update(Guid id, UpdateWorkoutPlan updateWorkoutPlan)
        {
            var entity = await _workoutPlanRepository.GetByIdAsync(id);
            if (entity == null)
                return null;

            entity.Description = updateWorkoutPlan.Description;

            var updated = await _workoutPlanRepository.UpdateAsync(entity);

            return MapToWorkoutPlan(updated);
        }

        public async Task Delete(Guid id)
        {
            await _workoutPlanRepository.DeleteAsync(id);
        }

        private static WorkoutPlan MapToWorkoutPlan(WorkoutPlanEntity w) => new()
        {
            Id = w.Id,
            UserId = w.UserId,
            Description = w.Description,
            CreatedAt = w.CreatedAt,
        };
    }
}
