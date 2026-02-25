using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class WorkoutExerciseInput
    {
        [Required(ErrorMessage = "WorkoutExercise id is required")]
        public Guid ExerciseId { get; set; } = Guid.Empty;

        [Required(ErrorMessage = "Exercise Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Exercise Amount must be positive")]
        public int SetAmount { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}
