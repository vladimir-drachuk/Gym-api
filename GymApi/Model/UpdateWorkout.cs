using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents data for updating a workout
    /// </summary>
    public class UpdateWorkout
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

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
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the workout plan identifier
        /// </summary>
        public Guid? WorkoutPlanId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the workout is completed
        /// </summary>
        public bool? IsCompleted { get; set; }
    }
}
