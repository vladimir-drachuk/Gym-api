using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class CreateWorkout
    {
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        public Guid? WorkoutPlanId { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime? Date { get; set; }

        public WorkoutExerciseInput[]? Exercises { get; set; }
    }
}
