using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class UpdateWorkout
    {
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public Guid? WorkoutPlanId { get; set; }
    }
}
