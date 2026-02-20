using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class Workout
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? WorkoutPlanId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now; 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class WorkoutExerciseInput
    {
        [Required(ErrorMessage = "WorkoutExercise id is required")]
        public Guid WorkoutExercise { get; set; } = Guid.Empty;

        [Required(ErrorMessage = "Exercise Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Exercise Amount must be positive1")]
        public int SetAmount { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }

    public class CreateWorkout : Workout
    {
        public new DateTime? Date { get; set; }
        public WorkoutExerciseInput[]? Exercises { get; set; }
    }

    public class UpdateWorkout
    {
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public Guid? WorkoutPlanId { get; set; }
    }

    public class UpdateWorkoutExercise
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
    }
}
