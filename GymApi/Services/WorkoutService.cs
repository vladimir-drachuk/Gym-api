using DataLayer.Entities;
using DataLayer.Interfaces;
using GymApi.Model;

namespace GymApi.Services
{
    public class WorkoutService(
        IWorkoutRepository workoutRepository,
        IExerciseRepository exerciseRepository,
        IWorkoutExerciseRepository workoutExerciseRepository,
        IWorkoutPlanRepository workoutPlanRepository,
        IUserRepository userRepository,
        ISetRepository setRepository
    )
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository = workoutExerciseRepository;
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISetRepository _setRepository = setRepository;

        public async Task<List<Workout>> GetAllByUserId(Guid userId)
        {
            var entities = await _workoutRepository.GetAllByUserIdAsync(userId);

            return [.. entities.Select(MapToWorkout)];
        }

        public async Task<List<Workout>> GetAllByWorkoutPlanId(Guid workoutPlanId)
        {
            var entities = await _workoutRepository.GetAllByWorkoutPlanIdAsync(workoutPlanId);

            return [.. entities.Select(MapToWorkout)];
        }

        public async Task<Workout?> GetByWorkoutId(Guid workoutId)
        {
            var entity = await _workoutRepository.GetByIdAsync(workoutId);

            return entity == null ? null : MapToWorkout(entity);
        }

        public async Task<Workout> CreateWorkout(CreateWorkout createWorkout)
        {
            _ = await _userRepository.GetByIdAsync(createWorkout.UserId)
                ?? throw new ArgumentException($"User '{createWorkout.UserId}' not found.");

            if (createWorkout.WorkoutPlanId.HasValue)
            {
                _ = await _workoutPlanRepository.GetByIdAsync(createWorkout.WorkoutPlanId.Value)
                    ?? throw new ArgumentException($"Workout plan '{createWorkout.WorkoutPlanId.Value}' not found.");
            }

            var exercisesInput = createWorkout.Exercises ?? [];

            var requestedIds = exercisesInput.Select(e => e.ExerciseId).Distinct().ToList();
            var isExerciseIdsCorrect = await _exerciseRepository.AnyAsync(e => requestedIds.Contains(e.Id));
            if (!isExerciseIdsCorrect)
            {
                throw new ArgumentException("One of the exercise ids is not correct or not exists");
            }

            var workoutEntity = new WorkoutEntity
            {
                UserId = createWorkout.UserId,
                WorkoutPlanId = createWorkout.WorkoutPlanId,
                Description = createWorkout.Description,
                Date = createWorkout.Date ?? DateTime.Now,
            };
            var createdWorkout = await _workoutRepository.AddAsync(workoutEntity);

            var (workoutExerciseEntities, setEntities) = GetWorkoutEntitiesFromInput(workoutEntity.Id, exercisesInput, startOrder: 1);

            await _workoutExerciseRepository.AddRangeAsync(workoutExerciseEntities);
            await _setRepository.AddRangeAsync(setEntities);

            return MapToWorkout(createdWorkout);
        }

        public async Task<Workout?> UpdateWorkout(Guid workoutId, UpdateWorkout updateWorkout)
        {
            var entity = await _workoutRepository.GetByIdAsync(workoutId);
            if (entity == null)
            {
                return null;
            }

            entity.Description = updateWorkout.Description;
            entity.Date = updateWorkout.Date;
            entity.WorkoutPlanId = updateWorkout.WorkoutPlanId;

            var updated = await _workoutRepository.UpdateAsync(entity);
            return MapToWorkout(updated);
        }

        public async Task<bool> UpdateWorkoutExercise(Guid workoutExerciseId, UpdateWorkoutExercise updateWorkoutExercise)
        {
            var entity = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId);
            if (entity == null)
                return false;

            entity.Description = updateWorkoutExercise.Description;

            await _workoutExerciseRepository.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteSet(Guid setId)
        {
            var entity = await _setRepository.GetByIdAsync(setId);
            if (entity == null)
                return false;

            var workoutExerciseId = entity.WorkoutExerciseId;

            await _setRepository.DeleteAsync(setId);
            await _setRepository.RecalculateOrderAfterDeleteAsync(workoutExerciseId);

            return true;
        }

        public async Task AddExercisesToWorkout(Guid workoutId, WorkoutExerciseInput[] workoutExercises)
        {
            _ = await _workoutRepository.GetByIdAsync(workoutId)
                 ?? throw new ArgumentException($"Workout '{workoutId}' not found.");

            var requestedIds = workoutExercises.Select(e => e.ExerciseId).Distinct().ToList();
            var isExerciseIdsCorrect = await _exerciseRepository.AnyAsync(e => requestedIds.Contains(e.Id));
            if (!isExerciseIdsCorrect)
            {
                throw new ArgumentException("One of the exercise ids is not correct or not exists");
            }

            var startOrder = await _workoutExerciseRepository.GetNextOrderAsync(workoutId);
            var (workoutExerciseEntities, setEntities) = GetWorkoutEntitiesFromInput(workoutId, workoutExercises, startOrder);

            await _workoutExerciseRepository.AddRangeAsync(workoutExerciseEntities);
            await _setRepository.AddRangeAsync(setEntities);
        }

        public async Task AddSetToWorkoutExercise(Guid workoutExerciseId, Set set)
        {
            _ = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId)
                ?? throw new ArgumentException($"WorkoutExercise '{workoutExerciseId}' not found.");

            var nextOrder = await _setRepository.GetNextOrderAsync(workoutExerciseId);

            var setEntity = new SetEntity
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = workoutExerciseId,
                Amount = set.Amount,
                Cheating = set.Cheating,
                Description = set.Description,
                Time = set.Time,
                Order = nextOrder,
            };

            await _setRepository.AddAsync(setEntity);
        }

        public async Task UpdateWorkoutExerciseSet(Guid workoutExerciseId, Set set)
        {
            var existing = await _setRepository.GetByIdAsync(set.Id)
                ?? throw new ArgumentException($"Set '{set.Id}' not found.");

            var setEntity = new SetEntity
            {
                Id = set.Id,
                WorkoutExerciseId = workoutExerciseId,
                Amount = set.Amount,
                Cheating = set.Cheating,
                Description = set.Description,
                Time = set.Time,
                Order = existing.Order,
            };
            await _setRepository.UpdateAsync(setEntity);
        }

        public async Task DeleteWorkoutExerciseFromWorkout(Guid workoutExerciseId)
        {
            var entity = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId);
            if (entity == null)
                return;

            var workoutId = entity.WorkoutId;

            await _workoutExerciseRepository.DeleteAsync(workoutExerciseId);
            await _workoutExerciseRepository.RecalculateOrderAfterDeleteAsync(workoutId);
        }

        public async Task DeleteWorkout(Guid workoutId)
        {
            await _workoutRepository.DeleteAsync(workoutId);
        }

        public async Task<bool> ReorderWorkoutExerciseAsync(Guid workoutExerciseId, int newOrder)
        {
            var entity = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId);
            if (entity == null)
                return false;

            await _workoutExerciseRepository.ReorderExerciseAsync(entity.WorkoutId, workoutExerciseId, entity.Order, newOrder);
            return true;
        }

        public async Task<Guid?> GetUserIdByWorkoutExerciseId(Guid workoutExerciseId)
        {
            return await _workoutExerciseRepository.GetUserIdByWorkoutExerciseIdAsync(workoutExerciseId);
        }

        public async Task<Guid?> GetUserIdByWorkoutId(Guid workoutId)
        {
            return await _workoutRepository.GetUserIdByWorkoutId(workoutId);
        }

        private static Workout MapToWorkout(WorkoutEntity w) => new()
        {
            Id = w.Id,
            UserId = w.UserId,
            WorkoutPlanId = w.WorkoutPlanId,
            Description = w.Description,
            Date = w.Date,
            CreatedAt = w.CreatedAt,
        };

        private static (IEnumerable<WorkoutExerciseEntity>, IEnumerable<SetEntity>) GetWorkoutEntitiesFromInput(
            Guid workoutId,
            WorkoutExerciseInput[] input,
            int startOrder)
        {
            var pairs = input.Select((we, index) =>
            {
                var weId = Guid.NewGuid();
                var exerciseOrder = startOrder + index;

                var workoutExercise = new WorkoutExerciseEntity
                {
                    Id = weId,
                    WorkoutId = workoutId,
                    ExerciseId = we.ExerciseId,
                    Description = we.Description ?? "",
                    Order = exerciseOrder,
                };

                var sets = Enumerable.Range(1, we.SetAmount)
                    .Select(setIndex => new SetEntity
                    {
                        Id = Guid.NewGuid(),
                        WorkoutExerciseId = weId,
                        Order = setIndex,
                    })
                    .ToList();

                return (WorkoutExercise: workoutExercise, Sets: sets);
            }).ToList();

            return (pairs.Select(p => p.WorkoutExercise), pairs.SelectMany(p => p.Sets));
        }
    }
}
