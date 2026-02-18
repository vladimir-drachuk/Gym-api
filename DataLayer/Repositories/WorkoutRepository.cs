using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class WorkoutRepository(GymDbContext dbContext) : BaseRepository<Workout>(dbContext), IWorkoutRepository
    {
        private new readonly DbSet<Workout> _dbSet = dbContext.Set<Workout>();

        public async Task<IEnumerable<Workout>> GetAllByUserIdAsync(Guid userId)
        {
            return await _dbSet.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetAllByWorkoutPlanIdAsync(Guid workoutPlanId)
        {
            return await _dbSet.Where(w => w.WorkoutPlanId == workoutPlanId).ToListAsync();
        }
    }
}
