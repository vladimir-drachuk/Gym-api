using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents input data for a workout exercise
    /// </summary>
    public class WorkoutExerciseInput
    {
        /// <summary>
        /// Gets or sets the exercise identifier
        /// </summary>
        [Required(ErrorMessage = "WorkoutExercise id is required")]
        public Guid ExerciseId { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the planned sets for this exercise
        /// </summary>
        [Required(ErrorMessage = "At least one set is required")]
        [MinLength(1, ErrorMessage = "At least one set is required")]
        public PlannedSetInput[] Sets { get; set; } = [];

        /// <summary>
        /// Gets or sets the exercise description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}
