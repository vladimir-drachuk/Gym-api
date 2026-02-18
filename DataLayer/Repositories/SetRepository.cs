using DataLayer.Entities;
using DataLayer.Interfaces;

namespace DataLayer.Repositories
{
    public class SetRepository(GymDbContext dbContext) : BaseRepository<Set>(dbContext), ISetRepository { }
}
