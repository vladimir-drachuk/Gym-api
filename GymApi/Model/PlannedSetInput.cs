using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents input data for a planned set
    /// </summary>
    public class PlannedSetInput
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
        /// Gets or sets the set description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}
