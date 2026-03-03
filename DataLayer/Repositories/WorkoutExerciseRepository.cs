using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class WorkoutExerciseRepository(GymDbContext dbContext) : BaseRepository<WorkoutExerciseEntity>(dbContext), IWorkoutExerciseRepository
    {
        private new readonly DbSet<WorkoutExerciseEntity> _dbSet = dbContext.Set<WorkoutExerciseEntity>();

        public async Task<Guid?> GetUserIdByWorkoutExerciseIdAsync(Guid workoutExerciseId)
        {
            return await _dbSet
                .Where(we => we.Id == workoutExerciseId)
                .Join(_dbContext.Workouts,
                    we => we.WorkoutId,
                    w => w.Id,
                    (we, w) => w.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetNextOrderAsync(Guid workoutId)
        {
            var maxOrder = await _dbSet
                .Where(we => we.WorkoutId == workoutId)
                .MaxAsync(we => (int?)we.Order);

            return (maxOrder ?? 0) + 1;
        }

        public async Task RecalculateOrderAfterDeleteAsync(Guid workoutId)
        {
            var exercises = await _dbSet
                .Where(we => we.WorkoutId == workoutId)
                .OrderBy(we => we.Order)
                .ToListAsync();

            for (var i = 0; i < exercises.Count; i++)
                exercises[i].Order = i + 1;

            _dbSet.UpdateRange(exercises);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ReorderExerciseAsync(Guid workoutId, Guid exerciseId, int currentOrder, int newOrder)
        {
            if (currentOrder == newOrder)
                return;

            if (newOrder < currentOrder)
            {
                // Shift exercises in [newOrder, currentOrder) up by 1
                await _dbSet
                    .Where(we => we.WorkoutId == workoutId
                              && we.Order >= newOrder
                              && we.Order < currentOrder)
                    .ExecuteUpdateAsync(s => s.SetProperty(we => we.Order, we => we.Order + 1));
            }
            else
            {
                // Shift exercises in (currentOrder, newOrder] down by 1
                await _dbSet
                    .Where(we => we.WorkoutId == workoutId
                              && we.Order > currentOrder
                              && we.Order <= newOrder)
                    .ExecuteUpdateAsync(s => s.SetProperty(we => we.Order, we => we.Order - 1));
            }

            await _dbSet
                .Where(we => we.Id == exerciseId)
                .ExecuteUpdateAsync(s => s.SetProperty(we => we.Order, newOrder));
        }

        public async Task<bool> AnyActualAsync(Guid workoutId)
        {
            return await _dbSet.AnyAsync(we => we.WorkoutId == workoutId && we.Date != null);
        }

        public async Task<WorkoutExerciseEntity?> FindActualByPlannedIdAsync(Guid plannedWorkoutExerciseId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(we => we.PlannedWorkoutExerciseId == plannedWorkoutExerciseId);
        }

        public async Task DeletePlannedByWorkoutIdAsync(Guid workoutId)
        {
            var plannedExercises = await _dbSet
                .Where(we => we.WorkoutId == workoutId && we.Date == null)
                .ToListAsync();

            if (plannedExercises.Any())
            {
                _dbSet.RemoveRange(plannedExercises);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteActualByWorkoutIdAsync(Guid workoutId)
        {
            var actualExercises = await _dbSet
                .Where(we => we.WorkoutId == workoutId && we.Date != null)
                .ToListAsync();

            if (actualExercises.Any())
            {
                _dbSet.RemoveRange(actualExercises);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<WorkoutExerciseEntity>> GetByWorkoutIdAsync(Guid workoutId)
        {
            return await _dbSet
                .Where(we => we.WorkoutId == workoutId)
                .OrderBy(we => we.Order)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkoutExerciseEntity>> GetByWorkoutIdWithDetailsAsync(Guid workoutId)
        {
            return await _dbSet
                .Where(we => we.WorkoutId == workoutId)
                .Include(we => we.Exercise)
                .Include(we => we.Sets.OrderBy(s => s.Order))
                .OrderBy(we => we.Order)
                .ToListAsync();
        }
    }
}
