namespace GymApi.Model
{
    /// <summary>
    /// Represents detailed information about a set
    /// </summary>
    public class SetDetail
    {
        /// <summary>
        /// Gets or sets the set identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the workout exercise identifier
        /// </summary>
        public Guid WorkoutExerciseId { get; set; }

        /// <summary>
        /// Gets or sets the target value for the set
        /// </summary>
        public decimal? Target { get; set; }

        /// <summary>
        /// Gets or sets the amount/reps for the set
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the time duration in seconds
        /// </summary>
        public int? Time { get; set; }

        /// <summary>
        /// Gets or sets the cheating array
        /// </summary>
        public int[]? Cheating { get; set; }

        /// <summary>
        /// Gets or sets the set description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the set order within the exercise
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the execution date (null for planned sets)
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the planned set identifier (for linking actual sets to planned sets)
        /// </summary>
        public Guid? PlannedSetId { get; set; }
    }
}
