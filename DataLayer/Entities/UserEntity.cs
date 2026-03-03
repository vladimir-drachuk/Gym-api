using DataLayer.Enums;

namespace DataLayer.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;

        public virtual ICollection<WorkoutPlanEntity> WorkoutPlans { get; set; } = new List<WorkoutPlanEntity>();

        public virtual ICollection<WorkoutEntity> Workouts { get; set; } = new List<WorkoutEntity>();

        // TrainerClient records where this user acts as the trainer
        public virtual ICollection<TrainerClientEntity> TrainerAssignments { get; set; } = new List<TrainerClientEntity>();

        // TrainerClient records where this user acts as the client
        public virtual ICollection<TrainerClientEntity> ClientAssignments { get; set; } = new List<TrainerClientEntity>();
    }
}
