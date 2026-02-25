using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class ReorderWorkoutExercise
    {
        [Required(ErrorMessage = "NewOrder is required")]
        [Range(1, int.MaxValue, ErrorMessage = "NewOrder must be positive")]
        public int NewOrder { get; set; }
    }
}
