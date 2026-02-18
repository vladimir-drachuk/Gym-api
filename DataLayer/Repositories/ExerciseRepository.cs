using DataLayer.Entities;
using DataLayer.Interfaces;

namespace DataLayer.Repositories
{
    public class ExerciseRepository(GymDbContext dbContext) : BaseRepository<Exercise>(dbContext), IExerciseRepository { }
}
