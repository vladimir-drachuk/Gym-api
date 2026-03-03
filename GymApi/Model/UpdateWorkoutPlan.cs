using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents data for updating a workout plan
    /// </summary>
    public class UpdateWorkoutPlan
    {
        /// <summary>
        /// Gets or sets the workout plan description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
