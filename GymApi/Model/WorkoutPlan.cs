namespace GymApi.Model
{
    public class WorkoutPlan
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateWorkoutPlan
    {
        public Guid UserId { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateWorkoutPlan
    {
        public string Description { get; set; } = string.Empty;
    }
}
