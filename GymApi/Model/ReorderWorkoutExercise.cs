using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents data for reordering a workout exercise
    /// </summary>
    public class ReorderWorkoutExercise
    {
        /// <summary>
        /// Gets or sets the new order position for the exercise
        /// </summary>
        [Required(ErrorMessage = "NewOrder is required")]
        [Range(1, int.MaxValue, ErrorMessage = "NewOrder must be positive")]
        public int NewOrder { get; set; }
    }
}
