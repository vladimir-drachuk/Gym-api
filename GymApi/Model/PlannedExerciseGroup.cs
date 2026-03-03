namespace GymApi.Model
{
    /// <summary>
    /// Represents a planned exercise with its optional linked actual exercise
    /// </summary>
    public class PlannedExerciseGroup
    {
        /// <summary>
        /// Gets or sets the planned exercise detail
        /// </summary>
        public WorkoutExerciseDetail Planned { get; set; } = null!;

        /// <summary>
        /// Gets or sets the actual exercise detail if executed, otherwise null
        /// </summary>
        public WorkoutExerciseDetail? Actual { get; set; }
    }
}
