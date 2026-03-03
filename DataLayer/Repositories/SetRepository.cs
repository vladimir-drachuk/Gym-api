using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class SetRepository(GymDbContext dbContext) : BaseRepository<SetEntity>(dbContext), ISetRepository
    {
        private new readonly DbSet<SetEntity> _dbSet = dbContext.Set<SetEntity>();

        public async Task<int> GetNextOrderAsync(Guid workoutExerciseId)
        {
            var maxOrder = await _dbSet
                .Where(s => s.WorkoutExerciseId == workoutExerciseId)
                .MaxAsync(s => (int?)s.Order);

            return (maxOrder ?? 0) + 1;
        }

        public async Task RecalculateOrderAfterDeleteAsync(Guid workoutExerciseId)
        {
            var sets = await _dbSet
                .Where(s => s.WorkoutExerciseId == workoutExerciseId)
                .OrderBy(s => s.Order)
                .ToListAsync();

            for (var i = 0; i < sets.Count; i++)
                sets[i].Order = i + 1;

            _dbSet.UpdateRange(sets);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<SetEntity>> GetByWorkoutExerciseIdsAsync(IEnumerable<Guid> workoutExerciseIds)
        {
            var idList = workoutExerciseIds.ToList();
            return await _dbSet
                .Where(s => idList.Contains(s.WorkoutExerciseId))
                .OrderBy(s => s.Order)
                .ToListAsync();
        }
    }
}
