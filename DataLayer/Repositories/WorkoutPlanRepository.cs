using DataLayer.Entities;
using DataLayer.Interfaces;

namespace DataLayer.Repositories
{
    public class WorkoutPlanRepository(GymDbContext dbContext) : BaseRepository<WorkoutPlanEntity>(dbContext), IWorkoutPlanRepository { }
}
