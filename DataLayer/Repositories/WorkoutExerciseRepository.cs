using DataLayer.Entities;
using DataLayer.Interfaces;

namespace DataLayer.Repositories
{
    public class WorkoutExerciseRepository(GymDbContext dbContext) : BaseRepository<WorkoutExercise>(dbContext), IWorkoutExerciseRepository { }
}
