namespace DataLayer.Entities
{
    public class WorkoutEntity : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid? WorkoutPlanId { get; set; }

        public string? Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public bool IsCompleted { get; set; } = false;

        public virtual UserEntity User { get; set; } = null!;

        public virtual WorkoutPlanEntity? WorkoutPlan { get; set; }

        public virtual ICollection<WorkoutExerciseEntity> WorkoutExercises { get; set; } = new List<WorkoutExerciseEntity>();
    }
}
