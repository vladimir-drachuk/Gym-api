using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents input data for adding a set to a workout exercise
    /// </summary>
    public class AddSetInput
    {
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
        /// Gets or sets the set description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the execution date
        /// </summary>
        [Required(ErrorMessage = "Date is required for actual sets")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the planned set identifier (for linking to planned sets)
        /// </summary>
        public Guid? PlannedSetId { get; set; }
    }
}
