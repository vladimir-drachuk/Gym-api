namespace GymApi.Model
{
    /// <summary>
    /// Represents the response containing workout exercises grouped by planned and free exercises
    /// </summary>
    public class WorkoutExercisesResponse
    {
        /// <summary>
        /// Gets or sets the list of planned exercises with optional linked actual exercises
        /// </summary>
        public List<PlannedExerciseGroup> Planned { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of free (unplanned) actual exercises
        /// </summary>
        public List<WorkoutExerciseDetail> Free { get; set; } = [];
    }
}
