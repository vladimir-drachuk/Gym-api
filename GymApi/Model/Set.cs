using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class Set
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "WorkoutExerciseId is required")]
        public Guid WorkoutExerciseId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount must be positive")]
        public int Amount { get; set; } = 1;
        
        public int? Time { get; set; }

        public int[]? Cheating { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
    }
}
