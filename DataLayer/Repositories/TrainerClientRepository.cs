using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class TrainerClientRepository(GymDbContext dbContext) : BaseRepository<TrainerClientEntity>(dbContext), ITrainerClientRepository
    {
        private new readonly DbSet<TrainerClientEntity> _dbSet = dbContext.Set<TrainerClientEntity>();

        public async Task<IEnumerable<TrainerClientEntity>> GetAllByTrainerIdAsync(Guid trainerId)
        {
            return await _dbSet.Where(tc => tc.TrainerId == trainerId).ToListAsync();
        }
    }
}
