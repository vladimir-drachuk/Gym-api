using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents input data for adding a free (unplanned) exercise to a workout
    /// </summary>
    public class AddFreeExerciseInput
    {
        /// <summary>
        /// Gets or sets the exercise identifier
        /// </summary>
        [Required(ErrorMessage = "ExerciseId is required")]
        public Guid ExerciseId { get; set; }

        /// <summary>
        /// Gets or sets the execution date
        /// </summary>
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the target value for the set
        /// </summary>
        public decimal? Target { get; set; }

        /// <summary>
        /// Gets or sets the amount/reps for the set
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be positive")]
        public int Amount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the time duration in seconds
        /// </summary>
        public int? Time { get; set; }

        /// <summary>
        /// Gets or sets the cheating array
        /// </summary>
        public int[]? Cheating { get; set; }

        /// <summary>
        /// Gets or sets the exercise description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}
