using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents data for creating a new workout plan
    /// </summary>
    public class CreateWorkoutPlan
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the workout plan description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
