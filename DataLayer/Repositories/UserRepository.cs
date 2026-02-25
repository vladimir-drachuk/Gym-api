using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class UserRepository(GymDbContext dbContext) : BaseRepository<UserEntity>(dbContext), IUserRepository
    {
        private new readonly DbSet<UserEntity> _dbSet = dbContext.Set<UserEntity>();

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
