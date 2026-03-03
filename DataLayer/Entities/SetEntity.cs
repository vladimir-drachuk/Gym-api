namespace DataLayer.Entities
{
    public class SetEntity : BaseEntity
    {
        public Guid WorkoutExerciseId { get; set; }

        public Guid? PlannedSetId { get; set; }

        public int Amount { get; set; } = 0;

        public int? Time { get; set; }

        public int[]? Cheating { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal? Target { get; set; }

        public int Order { get; set; }

        public DateTime? Date { get; set; }

        public virtual WorkoutExerciseEntity WorkoutExercise { get; set; } = null!;

        // Reference to the planned set this actual set was based on
        public virtual SetEntity? PlannedSet { get; set; }
    }
}
