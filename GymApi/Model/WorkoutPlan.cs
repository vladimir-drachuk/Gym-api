namespace GymApi.Model
{
    public class WorkoutPlan
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }
        
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
    }

    public class CreateWorkoutPlan
    {
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }
        
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateWorkoutPlan
    {
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
