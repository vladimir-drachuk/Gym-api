using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class WorkoutRepository(GymDbContext dbContext) : BaseRepository<WorkoutEntity>(dbContext), IWorkoutRepository
    {
        private new readonly DbSet<WorkoutEntity> _dbSet = dbContext.Set<WorkoutEntity>();

        public async Task<IEnumerable<WorkoutEntity>> GetAllByUserIdAsync(Guid userId)
        {
            return await _dbSet.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<WorkoutEntity>> GetAllByWorkoutPlanIdAsync(Guid workoutPlanId)
        {
            return await _dbSet.Where(w => w.WorkoutPlanId == workoutPlanId).ToListAsync();
        }

        public async Task<Guid?> GetUserIdByWorkoutId(Guid workoutId)
        {
            return await _dbSet
                .Where(w => w.Id == workoutId)
                .Select(w => w.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<WorkoutEntity?> GetWithExercisesAndSetsAsync(Guid workoutId)
        {
            return await _dbSet
                .Where(w => w.Id == workoutId)
                .Include(w => w.WorkoutExercises.OrderBy(we => we.Order))
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Sets.OrderBy(s => s.Order))
                .FirstOrDefaultAsync();
        }
    }
}
