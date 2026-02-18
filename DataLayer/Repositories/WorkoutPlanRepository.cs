using DataLayer.Entities;
using DataLayer.Interfaces;

namespace DataLayer.Repositories
{
    public class WorkoutPlanRepository(GymDbContext dbContext) : BaseRepository<WorkoutPlan>(dbContext), IWorkoutPlanRepository { }
}
