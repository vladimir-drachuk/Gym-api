using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents data for creating a new workout
    /// </summary>
    public class CreateWorkout
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the workout plan identifier
        /// </summary>
        public Guid? WorkoutPlanId { get; set; }

        /// <summary>
        /// Gets or sets the workout title
        /// </summary>
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the workout description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the workout date
        /// </summary>
        [Required(ErrorMessage = "Date is required")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the exercises to include in the workout
        /// </summary>
        public WorkoutExerciseInput[]? Exercises { get; set; }
    }
}
