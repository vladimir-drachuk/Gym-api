namespace DataLayer.Entities
{
    public class SetEntity : BaseEntity
    {
        public Guid WorkoutExerciseId { get; set; }

        public int Amount { get; set; } = 0;

        public int? Time { get; set; }

        public int[]? Cheating { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}
