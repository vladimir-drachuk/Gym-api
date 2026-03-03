namespace DataLayer.Entities
{
    public class WorkoutExerciseEntity : BaseEntity
    {
        public Guid WorkoutId { get; set; }

        public Guid? PlannedWorkoutExerciseId { get; set; }

        public Guid ExerciseId { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Order { get; set; }

        public DateTime? Date { get; set; }

        public virtual WorkoutEntity Workout { get; set; } = null!;

        public virtual ExerciseEntity Exercise { get; set; } = null!;

        // Reference to the planned exercise this actual exercise was based on
        public virtual WorkoutExerciseEntity? PlannedWorkoutExercise { get; set; }

        public virtual ICollection<SetEntity> Sets { get; set; } = new List<SetEntity>();
    }
}
