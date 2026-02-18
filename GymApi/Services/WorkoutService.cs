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
            var user = await _userRepository.GetByIdAsync(createWorkout.UserId)
                ?? throw new ArgumentException($"User '{createWorkout.UserId}' not found.");

            if (createWorkout.WorkoutPlanId.HasValue)
            {
                var workoutPlan = await _workoutPlanRepository.GetByIdAsync(createWorkout.WorkoutPlanId.Value)
                    ?? throw new ArgumentException($"Workou plan '{createWorkout.WorkoutPlanId.Value}' not found.");
            }

            var exercisesInput = createWorkout.Exercises ?? [];

            var requestedIds = exercisesInput.Select(e => e.WorkoutExercise).Distinct().ToList();
            var exercisesMap = (await _exerciseRepository.GetByIdsAsync(requestedIds))
                .ToDictionary(e => e.Id);

            var missingId = requestedIds.FirstOrDefault(id => !exercisesMap.ContainsKey(id));
            if (missingId != default)
                throw new ArgumentException($"Exercise '{missingId}' not found.");

            var workoutEntity = new DataLayer.Entities.Workout
            {
                UserId = createWorkout.UserId,
                WorkoutPlanId = createWorkout.WorkoutPlanId,
                Description = createWorkout.Description,
                Date = createWorkout.Date
            };
            var createdWorkout = await _workoutRepository.AddAsync(workoutEntity);

            var pairs = exercisesInput
                .Select(input =>
                {
                    var weId = Guid.NewGuid();

                    var workoutExercise = new DataLayer.Entities.WorkoutExercise
                    {
                        Id = weId,
                        WorkoutId = createdWorkout.Id,
                        ExerciseId = input.WorkoutExercise,
                    };

                    var sets = Enumerable.Range(0, input.SetAmount)
                        .Select(_ => new DataLayer.Entities.Set { Id = Guid.NewGuid(), WorkoutExerciseId = weId })
                        .ToList();

                    return (WorkoutExercise: workoutExercise, Sets: sets);
                })
                .ToList();

            var workoutExerciseEntities = pairs.Select(p => p.WorkoutExercise).ToList();
            var setEntities = pairs.SelectMany(p => p.Sets).ToList();

            await _workoutExerciseRepository.AddRangeAsync(workoutExerciseEntities);

            await _setRepository.AddRangeAsync(setEntities);

            return MapToWorkout(createdWorkout);
        }

        public async Task<Workout?> UpdateWorkout(Guid workoutId, UpdateWorkout updateWorkout)
        {
            var entity = await _workoutRepository.GetByIdAsync(workoutId);
            if (entity == null)
                return null;

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

            await _setRepository.DeleteAsync(setId);
            return true;
        }

        public async Task AddExercisesToWorkout(Guid workoutId, WorkoutExerciseInput[] workoutExercises)
        {
            var workout = await _workoutRepository.GetByIdAsync(workoutId)
                 ?? throw new ArgumentException($"Workout '{workoutId}' not found.");

            var requestedIds = workoutExercises.Select(e => e.WorkoutExercise).Distinct().ToList();
            var exercisesMap = (await _exerciseRepository.GetByIdsAsync(requestedIds))
                .ToDictionary(e => e.Id);

            var missingId = requestedIds.FirstOrDefault(id => !exercisesMap.ContainsKey(id));
            if (missingId != default)
                throw new ArgumentException($"Exercise '{missingId}' not found.");

            var pairs = workoutExercises.Select(we =>
            {
                var weId = Guid.NewGuid();

                var workoutExercise = new DataLayer.Entities.WorkoutExercise
                {
                    Id = weId,
                    WorkoutId = workoutId,
                    ExerciseId = we.WorkoutExercise,
                    Description = we.Description ?? "",
                };

                var sets = Enumerable.Range(0, we.SetAmount)
                    .Select(_ => new DataLayer.Entities.Set { Id = Guid.NewGuid(), WorkoutExerciseId = weId })
                    .ToList();

                return (WorkoutExercise: workoutExercise, Sets: sets);
            }).ToList();

            await _workoutExerciseRepository.AddRangeAsync(pairs.Select(p => p.WorkoutExercise));
            await _setRepository.AddRangeAsync(pairs.SelectMany(p => p.Sets));
        }

        public async Task AddSetToWorkoutExercise(Guid workoutExerciseId, Set set)
        {
            _ = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId)
                ?? throw new ArgumentException($"WorkoutExercise '{workoutExerciseId}' not found.");

            var setEntity = new DataLayer.Entities.Set
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = workoutExerciseId,
                Amount = set.Amount,
                Cheating = set.Cheating,
                Description = set.Description,
                Time = set.Time,
            };

            await _setRepository.AddAsync(setEntity);
        }

        public async Task UpdateWorkoutExerciseSet(Guid workoutExerciseId, Set set)
        {
            var setEntity = new DataLayer.Entities.Set
            {
                Id = set.Id,
                WorkoutExerciseId = workoutExerciseId,
                Amount = set.Amount,
                Cheating = set.Cheating,
                Description = set.Description,
                Time = set.Time,
            };
            await _setRepository.UpdateAsync(setEntity);
        }

        public async Task DeleteWorkoutExerciseFromWorkout(Guid workoutExerciseId)
        {
            await _workoutExerciseRepository.DeleteAsync(workoutExerciseId);
        }

        public async Task DeleteWorkout(Guid workoutId)
        {
            await _workoutRepository.DeleteAsync(workoutId);
        }

        private static Workout MapToWorkout(DataLayer.Entities.Workout w) => new()
        {
            Id = w.Id,
            UserId = w.UserId,
            WorkoutPlanId = w.WorkoutPlanId,
            Description = w.Description,
            Date = w.Date,
            CreatedAt = w.CreatedAt,
        };
    }
}
