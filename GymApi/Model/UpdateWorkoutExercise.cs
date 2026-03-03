using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents data for updating a workout exercise
    /// </summary>
    public class UpdateWorkoutExercise
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the exercise description
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
    }
}
