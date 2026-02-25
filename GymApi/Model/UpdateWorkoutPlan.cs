using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    public class UpdateWorkoutPlan
    {
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
