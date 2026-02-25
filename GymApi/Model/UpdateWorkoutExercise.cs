using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class UpdateWorkoutExercise
    {
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
    }
}
