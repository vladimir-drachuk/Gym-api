using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class UserRepository(GymDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
    {
        private new readonly DbSet<User> _dbSet = dbContext.Set<User>();

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
