using DataLayer.Entities;
using DataLayer.Interfaces;
using GymApi.Model;

namespace GymApi.Services
{
    /// <summary>
    /// Service for managing workouts, exercises, and sets
    /// </summary>
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

        /// <summary>
        /// Gets all workouts for a specific user
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns>List of workouts for the user</returns>
        public async Task<List<Workout>> GetAllByUserId(Guid userId)
        {
            var entities = await _workoutRepository.GetAllByUserIdAsync(userId);

            return [.. entities.Select(MapToWorkout)];
        }

        /// <summary>
        /// Gets all workouts for a specific workout plan
        /// </summary>
        /// <param name="workoutPlanId">The workout plan identifier</param>
        /// <returns>List of workouts for the workout plan</returns>
        public async Task<List<Workout>> GetAllByWorkoutPlanId(Guid workoutPlanId)
        {
            var entities = await _workoutRepository.GetAllByWorkoutPlanIdAsync(workoutPlanId);

            return [.. entities.Select(MapToWorkout)];
        }

        /// <summary>
        /// Gets a workout by its identifier
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>The workout if found, otherwise null</returns>
        public async Task<Workout?> GetByWorkoutId(Guid workoutId)
        {
            var entity = await _workoutRepository.GetByIdAsync(workoutId);

            return entity == null ? null : MapToWorkout(entity);
        }

        /// <summary>
        /// Creates a new workout with planned exercises and sets
        /// </summary>
        /// <param name="createWorkout">The workout creation data</param>
        /// <returns>The created workout</returns>
        /// <exception cref="ArgumentException">Thrown when user or workout plan not found, or exercise IDs are invalid</exception>
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
                Title = createWorkout.Title,
                Description = createWorkout.Description,
                Date = createWorkout.Date ?? DateTime.Now,
            };
            var createdWorkout = await _workoutRepository.AddAsync(workoutEntity);

            var (workoutExerciseEntities, setEntities) = GetWorkoutEntitiesFromInput(workoutEntity.Id, exercisesInput, startOrder: 1);

            await _workoutExerciseRepository.AddRangeAsync(workoutExerciseEntities);
            await _setRepository.AddRangeAsync(setEntities);

            return MapToWorkout(createdWorkout);
        }

        /// <summary>
        /// Updates workout metadata
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <param name="updateWorkout">The workout update data</param>
        /// <returns>The updated workout if found, otherwise null</returns>
        public async Task<Workout?> UpdateWorkout(Guid workoutId, UpdateWorkout updateWorkout)
        {
            var entity = await _workoutRepository.GetByIdAsync(workoutId);
            if (entity == null)
            {
                return null;
            }

            entity.Title = updateWorkout.Title;
            entity.Description = updateWorkout.Description;
            entity.Date = updateWorkout.Date;
            entity.WorkoutPlanId = updateWorkout.WorkoutPlanId;
            if (updateWorkout.IsCompleted.HasValue)
            {
                entity.IsCompleted = updateWorkout.IsCompleted.Value;
            }

            var updated = await _workoutRepository.UpdateAsync(entity);
            return MapToWorkout(updated);
        }

        /// <summary>
        /// Updates a workout exercise description
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        /// <param name="updateWorkoutExercise">The workout exercise update data</param>
        /// <returns>True if the exercise was updated, false if not found</returns>
        public async Task<bool> UpdateWorkoutExercise(Guid workoutExerciseId, UpdateWorkoutExercise updateWorkoutExercise)
        {
            var entity = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId);
            if (entity == null)
                return false;

            entity.Description = updateWorkoutExercise.Description;

            await _workoutExerciseRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Deletes an actual set
        /// </summary>
        /// <param name="setId">The set identifier</param>
        /// <returns>True if the set was deleted, false if not found</returns>
        /// <exception cref="ArgumentException">Thrown when trying to delete a planned set (only actual sets can be deleted)</exception>
        public async Task<bool> DeleteSet(Guid setId)
        {
            var entity = await _setRepository.GetByIdAsync(setId);
            if (entity == null)
                return false;

            if (!entity.Date.HasValue)
            {
                throw new ArgumentException("Can only delete actual sets.");
            }

            var workoutExerciseId = entity.WorkoutExerciseId;

            await _setRepository.DeleteAsync(entity);
            await _setRepository.RecalculateOrderAfterDeleteAsync(workoutExerciseId);

            return true;
        }

        /// <summary>
        /// Adds exercises to an existing workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <param name="workoutExercises">Array of exercises to add</param>
        /// <exception cref="ArgumentException">Thrown when workout not found or exercise IDs are invalid</exception>
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

        /// <summary>
        /// Adds a set to a workout exercise. Creates actual exercise if needed for planned exercises
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        /// <param name="setInput">The set data to add</param>
        /// <exception cref="ArgumentException">Thrown when workout exercise not found or invalid operation</exception>
        public async Task AddSetToWorkoutExercise(Guid workoutExerciseId, AddSetInput setInput)
        {
            var exercise = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId)
                ?? throw new ArgumentException($"WorkoutExercise '{workoutExerciseId}' not found.");

            // If this is a free actual exercise (no plan reference), add the set directly
            if (exercise.Date.HasValue && !exercise.PlannedWorkoutExerciseId.HasValue)
            {
                var nextOrderFree = await _setRepository.GetNextOrderAsync(exercise.Id);

                var freeSetEntity = new SetEntity
                {
                    Id = Guid.NewGuid(),
                    WorkoutExerciseId = exercise.Id,
                    PlannedSetId = null,
                    Target = setInput.Target,
                    Amount = setInput.Amount,
                    Cheating = setInput.Cheating,
                    Description = setInput.Description ?? string.Empty,
                    Time = setInput.Time,
                    Order = nextOrderFree,
                    Date = setInput.Date,
                };

                await _setRepository.AddAsync(freeSetEntity);
                return;
            }

            if (exercise.Date.HasValue)
            {
                throw new ArgumentException("Cannot add set to actual exercise. Use the planned exercise ID.");
            }

            var plannedExercise = exercise;
            var actualExercise = await _workoutExerciseRepository.FindActualByPlannedIdAsync(workoutExerciseId);
            
            if (actualExercise == null)
            {
                actualExercise = new WorkoutExerciseEntity
                {
                    Id = Guid.NewGuid(),
                    WorkoutId = plannedExercise.WorkoutId,
                    PlannedWorkoutExerciseId = plannedExercise.Id,
                    ExerciseId = plannedExercise.ExerciseId,
                    Description = plannedExercise.Description,
                    Order = plannedExercise.Order,
                    Date = DateTime.UtcNow,
                };
                await _workoutExerciseRepository.AddAsync(actualExercise);
            }

            var nextOrder = await _setRepository.GetNextOrderAsync(actualExercise.Id);

            var setEntity = new SetEntity
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = actualExercise.Id,
                PlannedSetId = setInput.PlannedSetId,
                Target = setInput.Target,
                Amount = setInput.Amount,
                Cheating = setInput.Cheating,
                Description = setInput.Description ?? string.Empty,
                Time = setInput.Time,
                Order = nextOrder,
                Date = setInput.Date,
            };

            await _setRepository.AddAsync(setEntity);
        }

        /// <summary>
        /// Updates an actual set
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        /// <param name="set">The set data to update</param>
        /// <exception cref="ArgumentException">Thrown when set not found or trying to update a planned set</exception>
        public async Task UpdateWorkoutExerciseSet(Guid workoutExerciseId, Set set)
        {
            var existing = await _setRepository.GetByIdAsync(set.Id)
                ?? throw new ArgumentException($"Set '{set.Id}' not found.");

            if (!existing.Date.HasValue)
            {
                throw new ArgumentException("Can only update actual sets.");
            }

            existing.WorkoutExerciseId = workoutExerciseId;
            existing.PlannedSetId = set.PlannedSetId ?? existing.PlannedSetId;
            existing.Target = set.Target;
            existing.Amount = set.Amount;
            existing.Cheating = set.Cheating;
            existing.Description = set.Description;
            existing.Time = set.Time;
            existing.Date = set.Date ?? existing.Date;

            await _setRepository.UpdateAsync(existing);
        }

        /// <summary>
        /// Deletes a free actual exercise from a workout
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        /// <exception cref="ArgumentException">Thrown when trying to delete a planned exercise or exercise linked to plan</exception>
        public async Task DeleteWorkoutExerciseFromWorkout(Guid workoutExerciseId)
        {
            var entity = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId);
            if (entity == null)
                return;

            if (!entity.Date.HasValue || entity.PlannedWorkoutExerciseId.HasValue)
            {
                throw new ArgumentException("Can only delete free actual exercises.");
            }

            var workoutId = entity.WorkoutId;

            await _workoutExerciseRepository.DeleteAsync(entity);
            await _workoutExerciseRepository.RecalculateOrderAfterDeleteAsync(workoutId);
        }

        /// <summary>
        /// Deletes a workout if it has no executed exercises
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <exception cref="ArgumentException">Thrown when workout has executed exercises</exception>
        public async Task DeleteWorkout(Guid workoutId)
        {
            var hasActual = await _workoutExerciseRepository.AnyActualAsync(workoutId);
            if (hasActual)
            {
                throw new ArgumentException("Cannot delete workout with executed exercises.");
            }

            await _workoutRepository.DeleteAsync(workoutId);
        }

        /// <summary>
        /// Reorders a workout exercise within its workout
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        /// <param name="newOrder">The new order position</param>
        /// <returns>True if the exercise was reordered, false if not found</returns>
        public async Task<bool> ReorderWorkoutExerciseAsync(Guid workoutExerciseId, int newOrder)
        {
            var entity = await _workoutExerciseRepository.GetByIdAsync(workoutExerciseId);
            if (entity == null)
                return false;

            await _workoutExerciseRepository.ReorderExerciseAsync(entity.WorkoutId, workoutExerciseId, entity.Order, newOrder);
            return true;
        }

        /// <summary>
        /// Gets the user identifier associated with a workout exercise
        /// </summary>
        /// <param name="workoutExerciseId">The workout exercise identifier</param>
        /// <returns>The user identifier if found, otherwise null</returns>
        public async Task<Guid?> GetUserIdByWorkoutExerciseId(Guid workoutExerciseId)
        {
            return await _workoutExerciseRepository.GetUserIdByWorkoutExerciseIdAsync(workoutExerciseId);
        }

        /// <summary>
        /// Gets the user identifier associated with a workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>The user identifier if found, otherwise null</returns>
        public async Task<Guid?> GetUserIdByWorkoutId(Guid workoutId)
        {
            return await _workoutRepository.GetUserIdByWorkoutId(workoutId);
        }

        /// <summary>
        /// Replaces the entire workout plan with new exercises and sets
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <param name="exercises">Array of new exercises to replace the plan</param>
        /// <exception cref="ArgumentException">Thrown when workout not found, has executed exercises, or exercise IDs are invalid</exception>
        public async Task ReplacePlan(Guid workoutId, WorkoutExerciseInput[] exercises)
        {
            _ = await _workoutRepository.GetByIdAsync(workoutId)
                ?? throw new ArgumentException($"Workout '{workoutId}' not found.");

            var hasActual = await _workoutExerciseRepository.AnyActualAsync(workoutId);
            if (hasActual)
            {
                throw new ArgumentException("Plan cannot be modified after execution started.");
            }

            var requestedIds = exercises.Select(e => e.ExerciseId).Distinct().ToList();
            var isExerciseIdsCorrect = await _exerciseRepository.AnyAsync(e => requestedIds.Contains(e.Id));
            if (!isExerciseIdsCorrect)
            {
                throw new ArgumentException("One of the exercise ids is not correct or not exists");
            }

            await _workoutExerciseRepository.DeletePlannedByWorkoutIdAsync(workoutId);

            var (workoutExerciseEntities, setEntities) = GetWorkoutEntitiesFromInput(workoutId, exercises, startOrder: 1);

            await _workoutExerciseRepository.AddRangeAsync(workoutExerciseEntities);
            await _setRepository.AddRangeAsync(setEntities);
        }

        /// <summary>
        /// Clears all actual (executed) data from a workout, keeping only the plan
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <exception cref="ArgumentException">Thrown when workout not found</exception>
        public async Task ClearActualData(Guid workoutId)
        {
            _ = await _workoutRepository.GetByIdAsync(workoutId)
                ?? throw new ArgumentException($"Workout '{workoutId}' not found.");

            await _workoutExerciseRepository.DeleteActualByWorkoutIdAsync(workoutId);
        }

        /// <summary>
        /// Gets workout exercises with sets, grouped into planned and free exercises
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <returns>The workout exercises response with planned and free exercises, or null if workout not found</returns>
        public async Task<WorkoutExercisesResponse?> GetWorkoutExercisesWithSets(Guid workoutId)
        {
            var workout = await _workoutRepository.GetWithExercisesAndSetsAsync(workoutId);

            if (workout == null)
                return null;

            var allDetails = workout.WorkoutExercises.Select(MapToExerciseDetail).ToList();

            var linkedActualsLookup = allDetails
                .Where(e => e.Date != null && e.PlannedWorkoutExerciseId != null)
                .ToLookup(e => e.PlannedWorkoutExerciseId!.Value);

            return new WorkoutExercisesResponse
            {
                Planned = allDetails
                    .Where(e => e.Date == null)
                    .Select(planned => new PlannedExerciseGroup
                    {
                        Planned = planned,
                        Actual = linkedActualsLookup[planned.Id].FirstOrDefault(),
                    })
                    .ToList(),
                Free = allDetails
                    .Where(e => e.Date != null && e.PlannedWorkoutExerciseId == null)
                    .ToList(),
            };
        }

        /// <summary>
        /// Maps a workout exercise entity to a workout exercise detail model
        /// </summary>
        /// <param name="we">The workout exercise entity</param>
        /// <returns>The mapped workout exercise detail</returns>
        private static WorkoutExerciseDetail MapToExerciseDetail(DataLayer.Entities.WorkoutExerciseEntity we) =>
            new()
            {
                Id = we.Id,
                ExerciseId = we.ExerciseId,
                ExerciseName = we.Exercise.Name,
                PlannedWorkoutExerciseId = we.PlannedWorkoutExerciseId,
                Description = we.Description,
                Order = we.Order,
                Date = we.Date,
                Sets = we.Sets.Select(s => new SetDetail
                {
                    Id = s.Id,
                    WorkoutExerciseId = s.WorkoutExerciseId,
                    Target = s.Target,
                    Amount = s.Amount,
                    Time = s.Time,
                    Cheating = s.Cheating,
                    Description = s.Description,
                    Order = s.Order,
                    Date = s.Date,
                    PlannedSetId = s.PlannedSetId,
                }).ToList(),
            };

        /// <summary>
        /// Adds a free (unplanned) exercise with an actual set to a workout
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <param name="input">The free exercise input data</param>
        /// <exception cref="ArgumentException">Thrown when workout or exercise not found</exception>
        public async Task AddFreeExerciseToWorkout(Guid workoutId, AddFreeExerciseInput input)
        {
            _ = await _workoutRepository.GetByIdAsync(workoutId)
                ?? throw new ArgumentException($"Workout '{workoutId}' not found.");

            _ = await _exerciseRepository.GetByIdAsync(input.ExerciseId)
                ?? throw new ArgumentException($"Exercise '{input.ExerciseId}' not found.");

            var nextOrder = await _workoutExerciseRepository.GetNextOrderAsync(workoutId);

            var freeExercise = new WorkoutExerciseEntity
            {
                Id = Guid.NewGuid(),
                WorkoutId = workoutId,
                PlannedWorkoutExerciseId = null,
                ExerciseId = input.ExerciseId,
                Description = input.Description ?? string.Empty,
                Order = nextOrder,
                Date = input.Date,
            };

            var createdExercise = await _workoutExerciseRepository.AddAsync(freeExercise);

            var freeSet = new SetEntity
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = createdExercise.Id,
                PlannedSetId = null,
                Target = input.Target,
                Amount = input.Amount,
                Cheating = input.Cheating,
                Description = input.Description ?? string.Empty,
                Time = input.Time,
                Order = 1,
                Date = input.Date,
            };

            await _setRepository.AddAsync(freeSet);
        }

        /// <summary>
        /// Maps a workout entity to a workout model
        /// </summary>
        /// <param name="w">The workout entity</param>
        /// <returns>The mapped workout</returns>
        private static Workout MapToWorkout(WorkoutEntity w) => new()
        {
            Id = w.Id,
            UserId = w.UserId,
            WorkoutPlanId = w.WorkoutPlanId,
            Title = w.Title,
            Description = w.Description,
            Date = w.Date,
            CreatedAt = w.CreatedAt,
            IsCompleted = w.IsCompleted,
        };

        /// <summary>
        /// Creates workout exercise and set entities from input data
        /// </summary>
        /// <param name="workoutId">The workout identifier</param>
        /// <param name="input">Array of workout exercise inputs</param>
        /// <param name="startOrder">The starting order for exercises</param>
        /// <returns>Tuple containing workout exercise entities and set entities</returns>
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
                    PlannedWorkoutExerciseId = null,
                    ExerciseId = we.ExerciseId,
                    Description = we.Description ?? "",
                    Order = exerciseOrder,
                    Date = null,
                };

                var sets = we.Sets.Select((setInput, setIndex) => new SetEntity
                    {
                        Id = Guid.NewGuid(),
                        WorkoutExerciseId = weId,
                        PlannedSetId = null,
                        Target = setInput.Target,
                        Amount = setInput.Amount,
                        Time = setInput.Time,
                        Description = setInput.Description ?? "",
                        Order = setIndex + 1,
                        Date = null,
                    })
                    .ToList();

                return (WorkoutExercise: workoutExercise, Sets: sets);
            }).ToList();

            return (pairs.Select(p => p.WorkoutExercise), pairs.SelectMany(p => p.Sets));
        }
    }
}
