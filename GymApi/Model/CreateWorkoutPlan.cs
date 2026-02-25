using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class CreateWorkoutPlan
    {
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
