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
        public Guid WorkoutExercise { get; set; } = Guid.Empty;
        public int SetAmount { get; set; }
        public string? Description { get; set; }
    }

    public class CreateWorkout : Workout
    {
        public WorkoutExerciseInput[]? Exercises { get; set; }
    }

    public class UpdateWorkout
    {
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid? WorkoutPlanId { get; set; }
    }

    public class UpdateWorkoutExercise
    {
        public string Description { get; set; } = string.Empty;
    }
}
