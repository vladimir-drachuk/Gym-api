namespace GymApi.Model
{
    /// <summary>
    /// Represents detailed information about a workout exercise
    /// </summary>
    public class WorkoutExerciseDetail
    {
        /// <summary>
        /// Gets or sets the workout exercise identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the exercise identifier
        /// </summary>
        public Guid ExerciseId { get; set; }

        /// <summary>
        /// Gets or sets the exercise name
        /// </summary>
        public string ExerciseName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the planned workout exercise identifier (null for free exercises)
        /// </summary>
        public Guid? PlannedWorkoutExerciseId { get; set; }

        /// <summary>
        /// Gets or sets the exercise description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exercise order within the workout
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the execution date (null for planned exercises)
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the list of sets for this exercise
        /// </summary>
        public List<SetDetail> Sets { get; set; } = [];
    }
}
