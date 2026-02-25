namespace DataLayer.Entities
{
    public class WorkoutExerciseEntity : BaseEntity
    {
        public Guid WorkoutId { get; set; }

        public Guid ExerciseId { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}
