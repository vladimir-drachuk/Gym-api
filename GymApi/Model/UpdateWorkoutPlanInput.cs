using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents input data for updating a workout plan with exercises
    /// </summary>
    public class UpdateWorkoutPlanInput
    {
        /// <summary>
        /// Gets or sets the array of exercises for the workout plan
        /// </summary>
        [Required(ErrorMessage = "Exercises array is required")]
        public WorkoutExerciseInput[] Exercises { get; set; } = [];
    }
}
