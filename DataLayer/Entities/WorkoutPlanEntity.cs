namespace DataLayer.Entities
{
    public class WorkoutPlanEntity : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Description { get; set; } = string.Empty;

        public virtual UserEntity User { get; set; } = null!;

        public virtual ICollection<WorkoutEntity> Workouts { get; set; } = new List<WorkoutEntity>();
    }
}
